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

    void Awake() {
      transform.Find("Package").gameObject.SetActive(true);
      transform.Find("Question").gameObject.SetActive(false);
      QEditorService.shared.LoadPackages();
    }

    void Update() {
      btnSavePackage.interactable = QEditorService.shared.packageDirty;
      btnSaveQuestions.interactable = QEditorService.shared.questionDirty;
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
      var package = new QEditorService.PackageItem();
      package.name = text;
      package.image = sprite.ToBase64();
      QEditorService.shared.AddPackage(package);
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

          break;
        case QEditorAction.ManagePakcage:
          QEditorService.shared.ManagerPackage(QEditorService.shared.GetPackageById(id));
          transform.Find("Package").gameObject.SetActive(false);
          transform.Find("Question").gameObject.SetActive(true);
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

    public void OnClickAddQuestion() {

    }

    public void SaveQuestions() {

    }

  }

}