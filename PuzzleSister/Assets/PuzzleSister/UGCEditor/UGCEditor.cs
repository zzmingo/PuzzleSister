using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SFB;
using PuzzleSister.QEditor;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class UGCEditor : MonoBehaviour {

    public void ShowUGCLegal() {
      SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
    }

    public void HandleAction(ActionType action, object data) {
      switch(action) {
        case ActionType.EditPackage: {
          var form = this.Query<PackageForm>("Package/Form");
          form.gameObject.SetActive(true);
          form.packageItem.Set((PackageItem) data);
          form.UpdateFormUI();
          break;
        }
        case ActionType.PublishOrUnpushlish: {
          PackageItem package = ((PackageItem) data).Clone();
          package.visible = !package.visible;
          StartCoroutine(PublishOrUnpublish(package));
          break;
        }
        case ActionType.ViewOnUGC: {
          PackageItem package = ((PackageItem) data).Clone();
          SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + package.publishedFileId.m_PublishedFileId);
          break;
        }
      }
    }

    IEnumerator Start() {
      Debug.Log(SteamUtils.GetAppID().m_AppId);
      UGCEditorLoading.shared.Show();
      yield return ReloadPackageList();
      UGCEditorLoading.shared.Hide();
    }

    public void CreateOrUpdatePackage() {
      var form = this.Query<PackageForm>("Package/Form");
      var package = form.packageItem.Clone();
      StartCoroutine(CreateOrUpdatePackageAsync(package));
      form.gameObject.SetActive(false);
      form.ResetFormData();
    }

    IEnumerator CreateOrUpdatePackageAsync(PackageItem package) {
      UGCEditorLoading.shared.Show();
      package.imagePath = WWW.UnEscapeURL(package.imagePath.Replace("file://", ""));
      if (package.publishedFileId == PublishedFileId_t.Invalid) {
        yield return UGCService.shared.CreatePackage(package);
      } else {
        yield return UGCService.shared.UpdatePackage(package);
      }
      yield return ReloadPackageList();
      UGCEditorLoading.shared.Hide();
    }

    IEnumerator PublishOrUnpublish(PackageItem package) {
      UGCEditorLoading.shared.Show();
      yield return UGCService.shared.UpdateVisibility(package);
      yield return ReloadPackageList();
      UGCEditorLoading.shared.Hide();
    }

    IEnumerator ReloadPackageList() {
      yield return UGCService.shared.LoadPackages();
      this.Query<PackageList>("Package/Table/Scroll View/Viewport/Content")
        .InitList(UGCService.shared.GetAllPackages());
    }
    

  }

}