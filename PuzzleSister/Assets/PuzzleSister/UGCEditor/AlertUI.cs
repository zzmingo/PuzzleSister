using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace PuzzleSister.UGCEditor {

  public class AlertUI : MonoBehaviour {

    public static AlertUI shared;

    private bool waitingConfirm;
    private bool confirmResult;

    void Awake() {
      shared = this;
    }

    public void Show(string text, string button = "OK") {
      transform.Find("AlertUI").gameObject.SetActive(true);
      transform.Query<Text>("AlertUI/Panel/Text").text = text;
      transform.Query<Text>("AlertUI/Panel/Btn/Text").text = button;
    }

    public IEnumerator Confirm(string text, Action<bool> callback) {
      transform.Find("ConfirmUI").gameObject.SetActive(true);
      transform.Query<Text>("ConfirmUI/Panel/Text").text = text;
      waitingConfirm = true;
      while(waitingConfirm) {
        yield return null;
      }
      callback(confirmResult);
    }

    public void SubmitConfirm(bool result) {
      waitingConfirm = false;
      confirmResult = result;
    }

  }

}