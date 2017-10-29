using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using PuzzleSister;
using System.Text;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class UGCService {

    public static UGCService shared = new UGCService();

    private List<PackageItem> packageList = new List<PackageItem>();

    public List<PackageItem> GetAllPackages() {
      return packageList;
    }

    public IEnumerator LoadPackages() {
      Debug.Log("load packages");

      bool packageLoaded = false;
      CallResult<SteamUGCQueryCompleted_t> callResult = CallResult<SteamUGCQueryCompleted_t>.Create((callback, ioFail) => {
        packageLoaded = true;
        var num = callback.m_unNumResultsReturned;
        packageList.Clear();
        for(uint i=0; i<num; i++) {
          PackageItem packageItem = new PackageItem();
          SteamUGCDetails_t details;
          SteamUGC.GetQueryUGCResult(callback.m_handle, i, out details);
          packageItem.publishedFileId = details.m_nPublishedFileId;
          packageItem.name = details.m_rgchTitle;
          string id;
          SteamUGC.GetQueryUGCMetadata(callback.m_handle, i, out id, Constants.k_cchDeveloperMetadataMax);
          packageItem.id = id;
          packageItem.description = details.m_rgchDescription;
          packageItem.visible = details.m_eVisibility == ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;
          SteamUGC.GetQueryUGCPreviewURL(callback.m_handle, i, out packageItem.imagePath, 1024);
          packageList.Add(packageItem);
          Debug.Log("query item: " + packageItem.name + ", " + packageItem.id + ", " + packageItem.description + ", " + packageItem.imagePath);
        }
        SteamUGC.ReleaseQueryUGCRequest(callback.m_handle);
      });

      UGCQueryHandle_t queryHandle = SteamUGC.CreateQueryUserUGCRequest(
        SteamUser.GetSteamID().GetAccountID(), 
        EUserUGCList.k_EUserUGCList_Published, 
        EUGCMatchingUGCType.k_EUGCMatchingUGCType_Items,
        EUserUGCListSortOrder.k_EUserUGCListSortOrder_CreationOrderDesc,
        AppId_t.Invalid,
        SteamUtils.GetAppID(),
        1
      );
      SteamUGC.SetReturnMetadata(queryHandle, true);
      callResult.Set(SteamUGC.SendQueryUGCRequest(queryHandle));

      while(!packageLoaded) {
        yield return null;
      }

      Debug.Log("loaded");
    }

    public IEnumerator CreatePackage(PackageItem package) {
      Debug.LogFormat("create package: {0},{1},{2}", package.name, package.description, package.imagePath);

      bool itemCreated = false;
      PublishedFileId_t publishedFileId = new PublishedFileId_t();
      CallResult<CreateItemResult_t> callResult = CallResult<CreateItemResult_t>.Create((callback, ioFail) => {
        if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement) {
          SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
        }
        publishedFileId = callback.m_nPublishedFileId;
        package.publishedFileId = publishedFileId;
        itemCreated = true;
      });
      callResult.Set(SteamUGC.CreateItem(SteamUtils.GetAppID(), EWorkshopFileType.k_EWorkshopFileTypeCommunity));

      while(!itemCreated) {
        yield return null;
      }

      yield return UpdatePackage(package);
    }

    public IEnumerator UpdatePackage(PackageItem package) {
      Debug.LogFormat("update package: {0},{1},{2}", package.name, package.description, package.imagePath);
      bool itemUpdated = false;

      // ensure id
      if (package.id == null) {
        package.id = System.Guid.NewGuid().ToString();
      }

      // ensure conten folder
      string contentDir = Utils.Path(Utils.GetAppInstallDir(), Const.UGC_CONTENT_DIR, package.id);
      if (!Directory.Exists(contentDir)) {
        Directory.CreateDirectory(contentDir);
      }

      var updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), package.publishedFileId);
      SteamUGC.SetItemTitle(updateHandle, package.name);
      SteamUGC.SetItemDescription(updateHandle, package.description);
      if (package.imagePath != null && !package.imagePath.StartsWith("http://") && !package.imagePath.StartsWith("https://") ) {
        SteamUGC.SetItemPreview(updateHandle, package.imagePath);
      }
      if (File.Exists(Utils.Path(contentDir, "Question.json"))) {
        SteamUGC.SetItemContent(updateHandle, contentDir);
      }
      SteamUGC.SetItemMetadata(updateHandle, package.id);
      SteamUGC.SetItemVisibility(updateHandle, 
        package.visible ? ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic : 
          ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
      var itemUpdateCallResult = CallResult<SubmitItemUpdateResult_t>.Create((callback, ioFail) => {
        Debug.Log("[" + SubmitItemUpdateResult_t.k_iCallback + " - SubmitItemUpdateResult] - " + callback.m_eResult + " -- " + callback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
        if (callback.m_eResult != EResult.k_EResultOK) {
          Debug.Log("update result: " + callback.m_eResult);
        }
        if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement) {
          SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
        }
        itemUpdated = true;
      });
      var itemUpdateApiCall = SteamUGC.SubmitItemUpdate(updateHandle, "Updated " + new DateTime().ToString());
      itemUpdateCallResult.Set(itemUpdateApiCall);

      while(!itemUpdated) {
        yield return null;
      }
    }

    public IEnumerator UpdateVisibility(PackageItem package) {
      bool itemUpdated = false;
      var updateHandle = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), package.publishedFileId);
      SteamUGC.SetItemVisibility(updateHandle, 
        package.visible ? ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic : 
          ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate);
      var itemUpdateCallResult = CallResult<SubmitItemUpdateResult_t>.Create((callback, ioFail) => {
        Debug.Log("[" + SubmitItemUpdateResult_t.k_iCallback + " - SubmitItemUpdateResult] - " + callback.m_eResult + " -- " + callback.m_bUserNeedsToAcceptWorkshopLegalAgreement);
        if (callback.m_eResult != EResult.k_EResultOK) {
          Debug.Log("update result: " + callback.m_eResult);
        }
        if (callback.m_bUserNeedsToAcceptWorkshopLegalAgreement) {
          SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
        }
        itemUpdated = true;
      });
      var itemUpdateApiCall = SteamUGC.SubmitItemUpdate(updateHandle, "Updated " + new DateTime().ToString());
      itemUpdateCallResult.Set(itemUpdateApiCall);

      while(!itemUpdated) {
        yield return null;
      }
    }

  }
  
}