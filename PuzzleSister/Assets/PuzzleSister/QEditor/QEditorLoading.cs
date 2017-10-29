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

  public class QEditorLoading : MonoBehaviour {

    public static QEditorLoading shared;

    void Awake() {
      shared = this;
    }

    public void Show() {
      transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Hide() {
      transform.GetChild(0).gameObject.SetActive(false);
    }

  }

}