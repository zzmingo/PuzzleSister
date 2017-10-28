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

  public class UGCUploader {

    public void UploadUGC() {
      var packageItem = QEditorService.shared.GetManagingPackage();
      var questionList = QEditorService.shared.GetQuestions();
      
    }

  }

}