using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SFB;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class UGCEditor : MonoBehaviour {

    void Start() {
      StartCoroutine(StartEditor());
    }

    IEnumerator StartEditor() {
      transform.Find("Package").gameObject.SetActive(true);
      transform.Find("Question").gameObject.SetActive(false);
      UGCEditorLoading.shared.Show();
      yield return ReloadPackageList();
      UGCEditorLoading.shared.Hide();
    }

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
          var package = UGCQuestionService.shared.GetPackage().Clone();
          package.visible = !package.visible;
          StartCoroutine(PublishOrUnpublish(package));
          break;
        }
        case ActionType.ViewOnUGC: {
          PackageItem package = ((PackageItem) data).Clone();
          SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + package.publishedFileId.m_PublishedFileId);
          break;
        }
        case ActionType.DeletePackage: {
          PackageItem package = ((PackageItem) data).Clone();

          break;
        }
        case ActionType.ManagePakcage: {
          PackageItem package = ((PackageItem) data).Clone();
          StartCoroutine(EditPackage(package));
          break;
        }
        case ActionType.EditQuestion: {
          Question question = ((Question) data).Clone();
          var form = this.Query<QuestionForm>("Question/Form");
          form.gameObject.SetActive(true);
          form.question = question;
          form.UpdateFormUI();
          break;
        }
      }
    }

    public void CreateOrUpdatePackage() {
      var form = this.Query<PackageForm>("Package/Form");
      var package = form.packageItem.Clone();
      StartCoroutine(CreateOrUpdatePackageAsync(package));
      form.gameObject.SetActive(false);
      form.ResetFormData();
    }

    public void AddOrUpdateQuestion() {
      var form = this.Query<QuestionForm>("Question/Form");
      var question = form.question.Clone();

      if (string.IsNullOrEmpty(question.title) || 
          string.IsNullOrEmpty(question.optionA) ||
          string.IsNullOrEmpty(question.optionB) ||
          string.IsNullOrEmpty(question.optionC) ||
          string.IsNullOrEmpty(question.optionD) ||
          string.IsNullOrEmpty(question.explain)
      ) {
        AlertUI.shared.Show("信息未填完整");
        return;
      }

      Debug.LogFormat("AddOrUpdateQuestion {0}", question.id);
      if (question.id == null) {
        UGCQuestionService.shared.AddQuestion(question);
        RefreshQuestionList();
      } else {
        UGCQuestionService.shared.UpdateQuestion(question);
        RefreshQuestionList();
      }

      form.gameObject.SetActive(false);
    }

    public void UpdatePackageToUGC() {
      var package = UGCQuestionService.shared.GetPackage().Clone();
      StartCoroutine(CreateOrUpdatePackageAsync(package));
    }

    public void PublishOrUnpublishToUGC() {
      var package = UGCQuestionService.shared.GetPackage().Clone();
      var questionList = UGCQuestionService.shared.GetQuestionList();
      if (!package.visible) {
        if (questionList.Count < 30 || questionList.Count > 100) {
          AlertUI.shared.Show("发布时的题目数量必须在30~100题");
          return;
        }
      }
      StartCoroutine(PublishOrUnpublish(package));
    }

    public void TryPackage() {
      AlertUI.shared.Show("暂不支持试玩");
    }

    public void Exit() {
      SceneManager.LoadScene("Main");
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
      UGCQuestionService.shared.GetPackage().visible = package.visible;
      transform.Query<Text>("Question/ActionBar/BtnPublish/Text").text = package.visible ? "下架" : "发布";
      UGCEditorLoading.shared.Hide();
    }

    IEnumerator ReloadPackageList() {
      yield return UGCService.shared.LoadPackages((error) => {
        Debug.Log(error);
      });
      if (transform.Find("Package").gameObject.activeInHierarchy) {
        this.Query<PackageList>("Package/Table/Scroll View/Viewport/Content")
          .InitList(UGCService.shared.GetAllPackages());
      }
    }
    
    IEnumerator EditPackage(PackageItem package) {
      UGCEditorLoading.shared.Show("加载数据中...");
      EResult result = EResult.k_EResultOK;
      yield return UGCQuestionService.shared.EditPackage(package, (error) => result = error);
      UGCEditorLoading.shared.Hide();
      if (result != EResult.k_EResultOK) {
        AlertUI.shared.Show("下载失败");
        yield break;
      }

      transform.Query<Text>("Question/ActionBar/BtnPublish/Text").text = package.visible ? "下架" : "发布";

      transform.Find("Question").gameObject.SetActive(true);
      transform.Find("Package").gameObject.SetActive(false);
      RefreshQuestionList();
    }

    void RefreshQuestionList() {
      var questionList = UGCQuestionService.shared.GetQuestionList();
      transform.Query<QuestionList>("Question/Table/Scroll View/Viewport/Content").InitList(questionList);
    }
    
  }

}