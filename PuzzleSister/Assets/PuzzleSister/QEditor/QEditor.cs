using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditor : MonoBehaviour {

    [NotNull] public GameObject oForm;

    void Awake() {
      QEditorService.shared.LoadPackages();
    }

    public void OnClickAddPackage() {
      string text = oForm.transform.Find("Content/InputField-Name").GetComponent<InputField>().text.Trim();
      Sprite sprite = oForm.transform.Find("Content/ImageArea/Image").GetComponent<Image>().sprite;
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
      oForm.SetActive(false);
      oForm.transform.Find("Content/InputField-Name").GetComponent<InputField>().text = "";
      oForm.transform.Find("Content/ImageArea/Image").GetComponent<Image>().sprite = null;
    }

    public void OnPackageItemAction(string id, QEditorAction action) {
      switch(action) {
        case QEditorAction.DeletePackage:
          // TODO check question
          QEditorService.shared.RemovePackageById(id);
          break;
        case QEditorAction.UpdatePackage:

          break;
      }
    }

  }

}