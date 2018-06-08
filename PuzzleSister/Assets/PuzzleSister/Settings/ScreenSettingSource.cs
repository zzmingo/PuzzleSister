using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {

  public class ScreenSettingSource : MonoBehaviour {

    void Awake() {
      var toggle = GetComponent<Toggle>();
      toggle.isOn = Settings.IsFullscreen();
      toggle.onValueChanged.AddListener((_) => {
        Settings.SetInt(Settings.FULLSCREEN, toggle.isOn ? 1 : 0);
        var resolution = Settings.ParseResolution(Settings.GetString(Settings.RESOLUTION, Settings.DEFAULT_RESOLUTION));
        Screen.SetResolution(resolution.width, resolution.height, toggle.isOn, resolution.refreshRate);
      });
    }

  }

}