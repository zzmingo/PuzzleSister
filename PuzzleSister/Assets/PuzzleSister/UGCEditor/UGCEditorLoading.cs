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

  public class UGCEditorLoading : MonoBehaviour {

    public static UGCEditorLoading shared;

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