using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister {

  public class ResolutionSettingSource : MonoBehaviour {

    void Awake() {
      var dropdown = GetComponent<Dropdown>();
      var resolutions = Settings.GetAvailableResolutions();
      List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
      foreach(var resol in resolutions) {
        string text = string.Format("{0}x{1}", resol.width, resol.height);
        options.Add(new Dropdown.OptionData(text));
      }
      dropdown.options = options;

      var resolution = Settings.GetString(Settings.RESOLUTION, "800x600");
      options = dropdown.options;
      for(int i=0; i<options.Count; i++) {
        var option = options[i];
        if (option.text.Equals(resolution)) {
          dropdown.value = i;
          break;
        }
      }
      dropdown.onValueChanged.AddListener((int value) => {
        var resolutionStr = dropdown.options[value].text;
        var resolutionVec = Settings.ParseResolutino(resolutionStr);
        Settings.SetString(Settings.RESOLUTION, resolutionStr);
        Screen.SetResolution(resolutionVec.x, resolutionVec.y, Settings.IsFullscreen(), 60);
      });
    }

  }

}