using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SFB;
using Newtonsoft.Json;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class UGCEditor : MonoBehaviour {

    ulong[] admins = new ulong[] {
      76561198256144099,
      76561198038776903
    };

    void Start() {
      StartCoroutine(StartEditor());
    }

    IEnumerator StartEditor() {
      transform.Find("Package").gameObject.SetActive(true);
      transform.Find("Question").gameObject.SetActive(false);
      UGCEditorLoading.shared.Show();
      yield return ReloadPackageList();
      UGCEditorLoading.shared.Hide();

      var steamId = SteamUser.GetSteamID().m_SteamID;
      Debug.Log(steamId);
      bool isAdmin = true;
      foreach(var admin in admins) {
        if (admin == steamId) {
          isAdmin = true;
        }
      }

      transform.Find("Question/ActionBar/BtnExport").gameObject.SetActive(isAdmin);
      transform.Find("Question/ActionBar/BtnImport").gameObject.SetActive(isAdmin);
    }

    public void ShowUGCLegal() {
      SteamFriends.ActivateGameOverlayToWebPage("http://steamcommunity.com/sharedfiles/workshoplegalagreement");
    }

    public void HandleAction(ActionType action, object data) {
      Debug.Log(action);
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
        case ActionType.DeleteQuestion: {
          Debug.Log("confirm");
          AlertUI.shared.Confirm("确定删除吗？", (ok) => {
            if (ok) {
              UGCQuestionService.shared.RemoveQuestion((Question) data);
              RefreshQuestionList();
            }
          });
          break;
        }
      }
    }

    public void CreateOrUpdatePackage() {
      var form = this.Query<PackageForm>("Package/Form");
      var package = form.packageItem.Clone();
      string errMsg = "";
      do {
        if (string.IsNullOrEmpty(package.name)) {
          errMsg = "请先输入题库名称";
          break;
        }
        if (string.IsNullOrEmpty(package.author)) {
          errMsg = "请先输入题库作者";
          break;
        }
        if (string.IsNullOrEmpty(package.description)) {
          errMsg = "请先输入题库描述";
          break;
        }
        if (string.IsNullOrEmpty(package.language)) {
          errMsg = "请先选择题库语言";
          break;
        }
        if (string.IsNullOrEmpty(package.imagePath)) {
          errMsg = "请先选择题库图片";
          break;
        }
      } while (false);
      if (!string.IsNullOrEmpty(errMsg)) {
        AlertUI.shared.Show(errMsg, "确定");
        return;
      }
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

    public void ExportQuestions() {
      if (UGCQuestionService.shared.GetQuestionList().Count <= 0) {
        return;
      }
      var path = StandaloneFileBrowser.SaveFilePanel("Export File", "", "exported", "puzzlesisters");
      if (!string.IsNullOrEmpty(path)) {
        Debug.LogFormat("export {0}", path);
        var questonList = UGCQuestionService.shared.GetQuestionList();
        var questionStr = JsonConvert.SerializeObject(questonList);
        questionStr = CryptoUtils.Encript(questionStr);
        File.WriteAllText(path, questionStr);
      }
    }

    public void ImportQuestions() {
      // ImportOldQuestions();
      var paths = StandaloneFileBrowser.OpenFilePanel("Import File", "", "puzzlesisters", false);
      if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0])) {
        var path = paths[0];
        if (path.StartsWith("file://")) {
          path = path.Replace("file://", "");
        }
        path = Uri.UnescapeDataString(path);
        Debug.LogFormat("import {0}", path);
        var questionStr = File.ReadAllText(path);
        questionStr = CryptoUtils.Decript(questionStr);
        var questionList = JsonConvert.DeserializeObject<List<Question>>(questionStr);
        Debug.LogFormat("import question: {0}", questionList.Count);
        if (questionList.Count >= 0) {
          UGCQuestionService.shared.AddQuestionList(questionList);
          RefreshQuestionList();
        }
      }
    }

    // public void ImportOldQuestions() {
    //   var paths = StandaloneFileBrowser.OpenFilePanel("Import File", "", "pzsister", false);
    //   if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0])) {
    //     var path = paths[0];
    //     if (path.StartsWith("file://")) {
    //       path = path.Replace("file://", "");
    //     }
    //     path = Uri.UnescapeDataString(path);
    //     Debug.LogFormat("import {0}", path);
    //     var questionStr = File.ReadAllText(path);
    //     var questionDictList = CSVUtils.Parse(questionStr);
    //     var questionList = new List<Question>();
    //     foreach(var row in questionDictList) {
    //       Question question = new Question();
    //       question.id = row["id"].ToString();
    //       question.title = row["title"].ToString();
    //       question.explain = row["explain"].ToString();
    //       Debug.Log(question.explain);
    //       question.result = (Question.Result) Enum.Parse(typeof(Question.Result), row["result"].ToString(), true);
    //       question.optionA = row["A"].ToString();
    //       question.optionB = row["B"].ToString();
    //       question.optionC = row["C"].ToString();
    //       question.optionD = row["D"].ToString();
    //       questionList.Add(question);
    //     }

    //     path = StandaloneFileBrowser.SaveFilePanel("Export File", "", "exported", "puzzlesisters");
    //     if (!string.IsNullOrEmpty(path)) {
    //       Debug.LogFormat("export {0}", path);
    //       questionStr = JsonConvert.SerializeObject(questionList);
    //       questionStr = CryptoUtils.Encript(questionStr);
    //       File.WriteAllText(path, questionStr);
    //     }
    //   }
    // }

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

		public void onBtnAddQeustionClick(GameObject formObj) {
			if (UGCQuestionService.shared.GetQuestionList().Count >= 100) {
				AlertUI.shared.Show("题目数量必须在30~100题");
				return;
			}
			formObj.SetActive(true);
			QuestionForm form = formObj.GetComponent<QuestionForm>();
			form.ResetDataAndUpdateUI();
		}
    
  }

}