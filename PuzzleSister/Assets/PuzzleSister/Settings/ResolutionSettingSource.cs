using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister {

  public class ResolutionSettingSource : MonoBehaviour {

		[NotNullAttribute]public GameObject label;

		void Awake() {
			label.GetComponent<Text>().text = Settings.GetString(Settings.RESOLUTION, Settings.DEFAULT_RESOLUTION);
    }

		private void ChangeResolution(Resolution resol) {
			if (resol.refreshRate == 0) {
				resol.refreshRate = 60;
			}
			var resolStr = string.Format("{0}x{1} ({2}hz)", resol.width, resol.height, resol.refreshRate);
			label.GetComponent<Text>().text = resolStr;
			Settings.SetString(Settings.RESOLUTION, resolStr);
			Screen.SetResolution(resol.width, resol.height, Settings.IsFullscreen());
		}

		public void OnLeftArrowClick() {
			var resolutionStr = label.GetComponent<Text>().text;
			var resol = Settings.ParseResolution(resolutionStr);
			var resolutions = Settings.GetAvailableResolutions();
			int index = 0;
			for (int i = 0, len = resolutions.Length; i < len; i++) {
				resolutions[i].refreshRate = 60;
				var resolution = resolutions[i];
				if (resolution.Equals(resol)) {
					if (i > 0) {
						index = i - 1;
					}
				}
			}
			resol = resolutions[index];
			ChangeResolution(resol);
		}

		public void onRightArrowClick() {
			var resolutionStr = label.GetComponent<Text>().text;
			var resol = Settings.ParseResolution(resolutionStr);
			var resolutions = Settings.GetAvailableResolutions();
			int index = 0;
			for (int i = 0, len = resolutions.Length; i < len; i++) {
				resolutions[i].refreshRate = 60;
				var resolution = resolutions[i];
				if (resolution.Equals(resol)) {
					if (i < len - 2) {
						index = i + 1;
					} else {
						index = len - 2;
					}
				}
			}
			resol = resolutions[index];
			ChangeResolution(resol);
		}

  }

}