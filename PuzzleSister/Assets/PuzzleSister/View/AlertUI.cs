using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace PuzzleSister {

  public class AlertUI : MonoBehaviour {

    public static AlertUI shared;

    private bool waitingConfirm;
    private bool confirmResult;
    private string btnText;

    void Awake() {
      shared = this;
    }

    public void Show(string text, string button = "OK", int time = 0) {
      transform.Find("AlertUI").gameObject.SetActive(true);
      transform.Query<Text>("AlertUI/Panel/Text").text = text;
      transform.Query<Text>("AlertUI/Panel/Btn/Text").text = button + "(" + time + "s)";
      if (time != 0) {
        this.btnText = button;
        StartCoroutine(Timer(time));
      }
    }

    private IEnumerator Timer(int time){
        while (time>0) {
            yield return new WaitForSeconds (1);
            time--;
            transform.Query<Text>("AlertUI/Panel/Btn/Text").text = this.btnText + "(" + time + "s)";
        }
        transform.Find("AlertUI").gameObject.SetActive(false);
    }

    public void Hide() {
      transform.Find("AlertUI").gameObject.SetActive(false);
      transform.Find("ConfirmUI").gameObject.SetActive(false);
    }

    public void Confirm(string text, Action<bool> callback) {
      StartCoroutine(ConfirmCO(text, callback));
    }
 
    public IEnumerator ConfirmCO(string text, Action<bool> callback) {
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