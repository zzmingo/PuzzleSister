using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister {

	public class ResolutionSettingSource : MonoBehaviour {

		void Awake() {
			var dropdown = GetComponent<Dropdown>();
			var resolutions = Settings.GetAvailableResolutions();
			var options = new List<Dropdown.OptionData>();
			var optionStrs = new List<string>();
			for(int i=0; i<resolutions.Length; i++) {
				resolutions[i].refreshRate = 60;
				var resol = resolutions[i];
				string text = string.Format("{0}x{1} ({2}hz)", resol.width, resol.height, resol.refreshRate);
				if (optionStrs.IndexOf(text) == -1) {
					options.Add(new Dropdown.OptionData(text));
					optionStrs.Add(text);
				}
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
				Screen.SetResolution(resol.width, resol.height, Settings.IsFullscreen());
			});
		}

	}

}