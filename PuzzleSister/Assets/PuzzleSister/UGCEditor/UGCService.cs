using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using PuzzleSister;
using System.Text;
using SFB;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor
{

    public class UGCService
    {

        public static UGCService shared = new UGCService();

        private List<PackageItem> packageList = new List<PackageItem>();

        private bool loaded = false;
        public bool Loaded
        {
            get
            {
                return loaded;
            }
        }

        public List<PackageItem> GetAllPackages()
        {
            return packageList;
        }

        public IEnumerator LoadSubscribed(Action<EResult> resultCb)
        {
            return LoadPackages(resultCb, EUserUGCList.k_EUserUGCList_Subscribed);
        }

        public IEnumerator LoadPackages(Action<EResult> resultCb)
        {
            return LoadPackages(resultCb, EUserUGCList.k_EUserUGCList_Published);
        }

        public IEnumerator LoadPackages(Action<EResult> resultCb, EUserUGCList eUserUGCList)
        {
            Debug.Log("load packages");

            Debug.Log("" + SteamUGC.GetNumSubscribedItems());

            bool packageLoaded = false;
            CallResult<SteamUGCQueryCompleted_t> callResult = CallResult<SteamUGCQueryCompleted_t>.Create((callback, ioFail) =>
            {
                if (ioFail)
                {
                    resultCb(EResult.k_EResultIOFailure);
                    return;
                }

                packageLoaded = true;
                var num = callback.m_unNumResultsReturned;
                packageList.Clear();
                for (uint i = 0; i < num; i++)
                {
                    PackageItem packageItem = new PackageItem();
                    SteamUGCDetails_t details;
                    SteamUGC.GetQueryUGCResult(callback.m_handle, i, out details);
                    packageItem.id = "" + details.m_nPublishedFileId.m_PublishedFileId;
                    packageItem.publishedFileId = details.m_nPublishedFileId;
                    packageItem.name = details.m_rgchTitle;
                    string metaData;
                    SteamUGC.GetQueryUGCMetadata(callback.m_handle, i, out metaData, Constants.k_cchDeveloperMetadataMax);
                    metaData.Trim('\0');
                    var tags = metaData.Split(',');
                    packageItem.author = tags.Length >= 2 ? tags[1].FromUnicodeString() : "???";
                    packageItem.language = tags[0];
                    packageItem.questionCount = 0;
                    if (tags.Length >= 3)
                    {
                        int.TryParse(tags[2], out packageItem.questionCount);
                    }
                    packageItem.description = details.m_rgchDescription;
                    packageItem.timeUpdated = details.m_rtimeUpdated;
                    packageItem.visible = details.m_eVisibility == ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;
                    SteamUGC.GetQueryUGCPreviewURL(callback.m_handle, i, out packageItem.imagePath, 1024);
                    packageList.Add(packageItem);
                    Debug.Log("query item: " + packageItem.name + ", " + packageItem.id + ", " + packageItem.description + ", " + packageItem.imagePath);
                }
                SteamUGC.ReleaseQueryUGCRequest(callback.m_handle);
            });

            UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUserUGCRequest(
              SteamUser.GetSteamID().GetAccountID(),
              eUserUGCList,
              EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items,
              EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc,
              AppId_t.Invalid,
              SteamUtils.GetAppID(),
              1
            );
            SteamUGC.SetReturnMetadata(queryHandle, true);
            callResult.Set(SteamUGC.SendQueryUGCRequest(queryHandle));

            while (!packageLoaded)
            {
                yield return null;
            }

            loaded = true;
            Debug.Log("loaded");
        }

        public IEnumerator CreatePackage(PackageItem package)
        {
            Debug.LogFormat("create package: {0},{1},{2},{3}", package.name, package.language, package.description, package.imagePath);

            bool itemCreated = false;
            var publishedFileId = new PublishedFileId_t();
            var callResult = CallResult<CreateItemResult_t>.Create((callback, ioFail) =>
            {
                if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
                {
                    SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
                }
                publishedFileId = callback.m_nPublishedFileId;
                package.id = "" + publishedFileId.m_PublishedFileId;
                package.questionCount = 0;
                package.publishedFileId = publishedFileId;
                package.visible = true;
                itemCreated = true;
            });
            callResult.Set(SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity));

            while (!itemCreated)
            {
                yield return null;
            }

            yield return UpdatePackage(package);
        }

        public IEnumerator UpdatePackage(PackageItem package)
        {
            Debug.LogFormat("update package: {0},{1},{2},{3}", package.name, package.language, package.description, package.imagePath);
            bool itemUpdated = false;

            // ensure conten folder
            string contentDir = Utils.Path(Utils.GetAppInstallDir(), Const.UGC_CONTENT_DIR, package.id);
            if (!Directory.Exists(contentDir))
            {
                Directory.CreateDirectory(contentDir);
            }
            var questionPath = Utils.Path(contentDir, Const.QUESTION_FILENAME);
            if (!File.Exists(questionPath))
            {
                Storage.shared.SerializeSave(questionPath, new List<Question>());
            }

            var updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), package.publishedFileId);
            SteamUGC.SetItemTitle(updateHandle, package.name);
            Debug.Log(package.author.ToUnicodeString());
            SteamUGC.SetItemMetadata(updateHandle, package.language + "," + package.author.ToUnicodeString() + "," + package.questionCount.ToString());
            SteamUGC.SetItemTags(updateHandle, null);
            //SteamUGC.SetItemTags(updateHandle, new List<string>{ package.language, package.author.ToUnicodeString(), package.questionCount.ToString() });
            SteamUGC.SetItemDescription(updateHandle, package.description);
            Debug.Log("image path: " + package.imagePath);
            if (package.imagePath != null && !package.imagePath.StartsWith("http://") && !package.imagePath.StartsWith("https://"))
            {
                SteamUGC.SetItemPreview(updateHandle, package.imagePath);
            }
            SteamUGC.SetItemContent(updateHandle, contentDir);
            SteamUGC.SetItemVisibility(updateHandle,
              package.visible ? ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic :
                ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
            var itemUpdateCallResult = CallResult<SubmitItemUpdateResult_t>.Create((callback, ioFail) =>
            {
                Debug.Log("[" + SubmitItemUpdateResult_t.k_iCallback + " - SubmitItemUpdateResult] - " + callback.m_eResult + " -- " + callback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
                if (callback.m_eResult != EResult.k_EResultOK)
                {
                    Debug.Log("update result: " + callback.m_eResult);
                }
                if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
                {
                    SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
                }
                itemUpdated = true;
            });
            var itemUpdateApiCall = SteamUGC.SubmitItemUpdate(updateHandle, "Updated " + new DateTime().ToString());
            itemUpdateCallResult.Set(itemUpdateApiCall);

            while (!itemUpdated)
            {
                yield return null;
            }
        }

        public IEnumerator UpdateVisibility(PackageItem package)
        {
            bool itemUpdated = false;
            var updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), package.publishedFileId);
            SteamUGC.SetItemVisibility(updateHandle,
              package.visible ? ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic :
                ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
            var itemUpdateCallResult = CallResult<SubmitItemUpdateResult_t>.Create((callback, ioFail) =>
            {
                Debug.Log("[" + SubmitItemUpdateResult_t.k_iCallback + " - SubmitItemUpdateResult] - " + callback.m_eResult + " -- " + callback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
                if (callback.m_eResult != EResult.k_EResultOK)
                {
                    Debug.Log("update result: " + callback.m_eResult);
                }
                if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement)
                {
                    SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
                }
                itemUpdated = true;
            });
            var itemUpdateApiCall = SteamUGC.SubmitItemUpdate(updateHandle, "Updated " + new DateTime().ToString());
            itemUpdateCallResult.Set(itemUpdateApiCall);

            while (!itemUpdated)
            {
                yield return null;
            }
        }

    }

}