using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditor : MonoBehaviour {

    [NotNull] public GameObject oPackageForm;
    [NotNull] public Button btnSavePackage;

    [NotNull] public GameObject oQuestionForm;
    [NotNull] public Button btnSaveQuestions;

    private QEditorService.PackageItem editingPackage;
    private Question editingQuestion;

    void Awake() {
      transform.Find("Package").gameObject.SetActive(true);
      transform.Find("Question").gameObject.SetActive(false);
      oPackageForm.SetActive(false);
      oQuestionForm.SetActive(false);
      QEditorService.shared.LoadPackages();
    }

    void Update() {
      btnSavePackage.interactable = QEditorService.shared.packageDirty;
      btnSaveQuestions.interactable = QEditorService.shared.questionDirty;
    }

    public void OpenPackageForm(string packageId = null) {
      oPackageForm.SetActive(true);
      bool editing = !string.IsNullOrEmpty(packageId.Trim());
      editingPackage = editing ? QEditorService.shared.GetPackageById(packageId) : null;
      oPackageForm.Query<InputField>("Content/InputField-Name").text = editing ? editingPackage.name : "";
      oPackageForm.Query<Image>("Content/ImageArea/Image").sprite = editing ? editingPackage.image.ToSprite() : null;
    }

    public void OnClickAddPackage() {
      string text = oPackageForm.transform.Find("Content/InputField-Name").GetComponent<InputField>().text.Trim();
      Sprite sprite = oPackageForm.transform.Find("Content/ImageArea/Image").GetComponent<Image>().sprite;
      if (string.IsNullOrEmpty(text)) {
        return;
      }
      if (sprite == null) {
        return;
      }
      var package = editingPackage != null ? editingPackage : new QEditorService.PackageItem();
      package.name = text;
      package.image = sprite.ToBase64();
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
          QEditorService.shared.ManagerPackage(QEditorService.shared.GetPackageById(id));
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

  }

}