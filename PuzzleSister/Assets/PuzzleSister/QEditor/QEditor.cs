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

namespace PuzzleSister.QEditor {

  public class QEditor : MonoBehaviour {

    [NotNull] public GameObject oPackageForm;
    [NotNull] public GameObject oQuestionForm;

    private QEditorService.PackageItem editingPackage;
    private Question editingQuestion;

    void Awake() {
      transform.Find("Package").gameObject.SetActive(true);
      transform.Find("Question").gameObject.SetActive(false);
      oPackageForm.SetActive(false);
      oQuestionForm.SetActive(false);
      QEditorService.shared.LoadPackages();
    }

    public void Exit() {
      if (QEditorService.shared.packageDirty || QEditorService.shared.questionDirty) {
        QEditorAlertUI.shared.Show("你有信息未保存");
        return;
      }
      SceneManager.LoadScene("Main");
    }

    public void OpenPackageForm(string packageId = null) {
      oPackageForm.SetActive(true);
      bool editing = !string.IsNullOrEmpty(packageId.Trim());
      editingPackage = editing ? QEditorService.shared.GetPackageById(packageId) : null;
      string author = "";
#if UNITY_STANDALONE
      if (editingPackage == null || string.IsNullOrEmpty(editingPackage.author)) { 
        author = SteamFriends.GetPersonaName();
      } else {
        author = editing ? editingPackage.author : "";
      }
#endif
      oPackageForm.Query<InputField>("Content/InputField-Name").text = editing ? editingPackage.name : "";
      oPackageForm.Query<InputField>("Content/InputField-Author").text = author;
      oPackageForm.Query<InputField>("Content/InputField-Description").text = editing ? editingPackage.description : "";
      oPackageForm.Query<Image>("Content/ImageArea/Image").sprite = editing ? editingPackage.thumb.ToSprite() : null;
    }

    public void OnClickAddPackage() {
      string text = oPackageForm.transform.Find("Content/InputField-Name").GetComponent<InputField>().text.Trim();
      string author = oPackageForm.transform.Find("Content/InputField-Author").GetComponent<InputField>().text.Trim();
      string description = oPackageForm.transform.Find("Content/InputField-Description").GetComponent<InputField>().text.Trim();
      Sprite sprite = oPackageForm.transform.Find("Content/ImageArea/Image").GetComponent<Image>().sprite;
      if (string.IsNullOrEmpty(text) ||
        string.IsNullOrEmpty(author) ||
        string.IsNullOrEmpty(description) ||
        sprite == null) 
      {
        QEditorAlertUI.shared.Show("信息未填完整");
        return;
      }
      var package = editingPackage != null ? editingPackage : new QEditorService.PackageItem();
      package.name = text;
      package.author = author;
      package.description = description;
      package.thumb = sprite.ToBase64();
      if (editingPackage != null) {
        QEditorService.shared.UpdatePackage(editingPackage);
        editingPackage = null;
      } else {
        QEditorService.shared.AddPackage(package);
      }
      oPackageForm.SetActive(false);
      oPackageForm.transform.Find("Content/InputField-Name").GetComponent<InputField>().text = "";
      oPackageForm.transform.Find("Content/ImageArea/Image").GetComponent<Image>().sprite = null;
    }

    public void OnPackageItemAction(string id, QEditorAction action) {
      switch(action) {
        case QEditorAction.DeletePackage:
          // TODO check question
          QEditorService.shared.RemovePackageById(id);
          break;
        case QEditorAction.UpdatePackage:
          OpenPackageForm(id);
          break;
        case QEditorAction.ManagePakcage:
          transform.Find("Package").gameObject.SetActive(false);
          transform.Find("Question").gameObject.SetActive(true);
          QEditorService.shared.ManagePackage(QEditorService.shared.GetPackageById(id));
          break;
        case QEditorAction.DeleteQuestion:
          QEditorService.shared.RemoveQuestionById(id);
          break;
        case QEditorAction.UpdateQuestion:
          OpenQuestionForm(id);
          break;
      }
    }

    public void BackToPackagePanel() {
      if (QEditorService.shared.questionDirty) {
        QEditorAlertUI.shared.Show("你有信息未保存");
        return;
      }
      transform.Find("Package").gameObject.SetActive(true);
      transform.Find("Question").gameObject.SetActive(false);
    }

    public void SavePackages() {
      QEditorService.shared.SavePackages();
    }

    public void OpenQuestionForm(string questionId = null) {
      oQuestionForm.SetActive(true);

      bool adding = string.IsNullOrEmpty(questionId.Trim());
      var question = adding ? null : QEditorService.shared.GetQuestionById(questionId);
      oQuestionForm.Query<InputField>("Content/InputField-Title").text = adding ? "" : question.title;
      oQuestionForm.Query<InputField>("Content/InputField-A").text = adding ? "" : question.optionA;
      oQuestionForm.Query<InputField>("Content/InputField-B").text = adding ? "" : question.optionB;
      oQuestionForm.Query<InputField>("Content/InputField-C").text = adding ? "" : question.optionC;
      oQuestionForm.Query<InputField>("Content/InputField-D").text = adding ? "" : question.optionD;
      oQuestionForm.Query<InputField>("Content/InputField-Explain").text = adding ? "" : question.explain;

      var toggleGroup = oQuestionForm.Query<ToggleGroup>("Content/Result");
      var toggle = oQuestionForm.Query<Toggle>("Content/Result/" + (adding ? "A" : question.result.Name()));
      toggleGroup.NotifyToggleOn(toggle);

      editingQuestion = question;
    }

    public void OnClickAddQuestion() {

      string text = oQuestionForm.Query<InputField>("Content/InputField-Title").text.Trim();
      string optionA = oQuestionForm.Query<InputField>("Content/InputField-A").text.Trim();
      string optionB = oQuestionForm.Query<InputField>("Content/InputField-B").text.Trim();
      string optionC = oQuestionForm.Query<InputField>("Content/InputField-C").text.Trim();
      string optionD = oQuestionForm.Query<InputField>("Content/InputField-D").text.Trim();
      string explain = oQuestionForm.Query<InputField>("Content/InputField-Explain").text.Trim();
      string result = "";
      
      foreach(var toggle in oQuestionForm.Query<ToggleGroup>("Content/Result").ActiveToggles()) {
        if (toggle.isOn) {
          result = toggle.name;
          break;
        }
      }
      if (string.IsNullOrEmpty(text) || 
          string.IsNullOrEmpty(optionA) ||
          string.IsNullOrEmpty(optionB) ||
          string.IsNullOrEmpty(optionC) ||
          string.IsNullOrEmpty(optionD) ||
          string.IsNullOrEmpty(explain) ||
          string.IsNullOrEmpty(result)
      ) {
        QEditorAlertUI.shared.Show("信息未填完整");
        return;
      }

      var question = editingQuestion == null ? new Question() : editingQuestion;
      question.title = text;
      question.optionA = optionA;
      question.optionB = optionB;
      question.optionC = optionC;
      question.optionD = optionD;
      question.explain = explain;
      question.result = (Question.Result) Enum.Parse(typeof(Question.Result), result);

      if (editingQuestion == null) {
        QEditorService.shared.AddQuestion(question);
      } else {
        QEditorService.shared.UpdateQuestion(question);
      }

      foreach(var inputField in oQuestionForm.GetComponentsInChildren<InputField>()) {
        inputField.text = "";
      }
      oQuestionForm.SetActive(false);
    }

    public void SaveQuestions() {
      QEditorService.shared.SaveQuestions();
    }

    public void TryPackage() {
      var packageItem = QEditorService.shared.GetManagingPackage();
      var questionList = QEditorService.shared.GetQuestions();
      if (questionList.Count <= 0) {
        QEditorAlertUI.shared.Show("请先添加题目");
        return;
      }
      var pkg = new Package();
      pkg.id = packageItem.id;
      pkg.name = packageItem.name;
      pkg.thumb = packageItem.thumb;
      pkg.source = Package.Source.Memory;
      pkg.type = Package.Type.None;
      pkg.temporary = true;
      pkg.questionList = QEditorService.shared.GetQuestions();
      Repository.shared.ReplacePackage(pkg);
      SceneManager.LoadScene("Main");
    }

    public void ExportPackage() {
      var packageItem = QEditorService.shared.GetManagingPackage();
      var questionList = QEditorService.shared.GetQuestions();
      if (questionList.Count <= 0) {
        QEditorAlertUI.shared.Show("请先添加题目");
        return;
      }

      var pkgCSVStr = String.Format(
        "id,name,author,description,thumb\n\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\"", 
        packageItem.id, packageItem.name, packageItem.author, packageItem.description, packageItem.thumb
      );
      
      var questionStrList = new List<string>();
      questionStrList.Add("id,title,A,B,C,D,result,explain");
      questionList.ForEach((question) => {
        questionStrList.Add(string.Format(
          "{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\"",
          question.id,
          question.title,
          question.optionA,
          question.optionB,
          question.optionC,
          question.optionD,
          question.result,
          question.explain
        ));
      });
      var questionCSVStr = string.Join("\n", questionStrList.ToArray());

      var extensions = new [] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )
      };
      var path = StandaloneFileBrowser.SaveFilePanel("导出题库", "", "Export-" + packageItem.name, "pzsister");
      if (string.IsNullOrEmpty(path)) {
        return;
      }
      File.WriteAllText(path, pkgCSVStr + "\n---\n" + questionCSVStr);
    }

  }

}