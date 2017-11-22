using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister {

  public class ResolutionSettingSource : MonoBehaviour {

    void Awake() {
      var dropdown = GetComponent<Dropdown>();
      var resolutions = Settings.GetAvailableResolutions();
      var options = new List<Dropdown.OptionData>();
      for(int i=0; i<resolutions.Length; i++) {
        var resol = resolutions[i];
        if (resol.refreshRate == 0) {
          resol.refreshRate = 60;
        }
        string text = string.Format("{0}x{1} ({2}hz)", resol.width, resol.height, resol.refreshRate);
        options.Add(new Dropdown.OptionData(text));
      }
      dropdown.options = options;

      var resolution = Settings.GetString(Settings.RESOLUTION, Settings.DEFAULT_RESOLUTION);
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
        var resol = Settings.ParseResolution(resolutionStr);
        Settings.SetString(Settings.RESOLUTION, resolutionStr);
        Screen.SetResolution(resol.width, resol.height, Settings.IsFullscreen(), resol.refreshRate);
      });
    }

  }

}