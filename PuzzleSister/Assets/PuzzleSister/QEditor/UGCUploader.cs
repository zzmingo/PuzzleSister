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

  public class UGCUploader : MonoBehaviour {

    public static UGCUploader shared;

    void Awake() {
      shared = this;
    }

    public void UploadUGC() {
      var packageItem = QEditorService.shared.GetManagingPackage();
      var questionList = QEditorService.shared.GetQuestions();
      if (questionList.Count < 30 || questionList.Count > 100) {
        QEditorAlertUI.shared.Show("上传工坊前，题目数量必须在30~100题内");
        return;
      }
      
      transform.GetChild(0).gameObject.SetActive(true);
      transform.Query<Button>("UploadUI/Panel/Btn").gameObject.SetActive(false);

    }

    void OnCreateItemCallback(UGCQueryHandle_t handle) {

    }

  }

}