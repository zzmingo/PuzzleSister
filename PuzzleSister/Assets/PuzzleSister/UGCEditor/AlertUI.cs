using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister.UGCEditor {

  public class AlertUI : MonoBehaviour {

    public static AlertUI shared;

    void Awake() {
      shared = this;
    }

    public void Show(string text, string button = "OK") {
      transform.GetChild(0).gameObject.SetActive(true);
      transform.Query<Text>("AlertUI/Panel/Text").text = text;
      transform.Query<Text>("AlertUI/Panel/Button/Text").text = text;
    }

  }

}