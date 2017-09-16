using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {

  public class ScreenSettingSource : MonoBehaviour {

    void Awake() {
      var toggle = GetComponent<Toggle>();
      var label = transform.Find("Label").GetComponent<Text>();
      toggle.isOn = Settings.IsFullscreen();
      label.text = toggle.isOn ? "全屏" : "窗口化";
      toggle.onValueChanged.AddListener((_) => {
        Settings.SetInt(Settings.FULLSCREEN, toggle.isOn ? 1 : 0);
        Screen.fullScreen = toggle.isOn;
        label.text = toggle.isOn ? "全屏" : "窗口化";
      });
    }

  }

}