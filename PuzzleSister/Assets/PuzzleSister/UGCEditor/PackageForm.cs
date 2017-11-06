using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using PuzzleSister;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class PackageForm : MonoBehaviour {

    public readonly PackageItem packageItem = new PackageItem();

    private bool blockingFormData = false;

    public void ResetFormData() {
      packageItem.name = null;
      packageItem.description = null;
      packageItem.imagePath = null;
    }

    public void UpdateFormUI() {
      blockingFormData = true;
      this.Query<InputField>("Content/InputField-Name").text = packageItem.name;
      this.Query<InputField>("Content/InputField-Description").text = packageItem.description;
      this.Query<ImageSelector>("Content/ImageArea").imagePath = packageItem.imagePath;
      this.Query<ImageSelector>("Content/ImageArea").UpdateImage();
      blockingFormData = false;
    }

    public void UpdateFormData() {
      if (blockingFormData) {
        return;
      }
      packageItem.name = this.Query<InputField>("Content/InputField-Name").text;
      packageItem.description = this.Query<InputField>("Content/InputField-Description").text;
      packageItem.imagePath = this.Query<ImageSelector>("Content/ImageArea").imagePath;
    }

  }

}