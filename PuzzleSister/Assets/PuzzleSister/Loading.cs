using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleSister {

	public class Loading : MonoBehaviour {

		private static bool loaded = false;

		public static void Load() {
			if (loaded) {
				return;
			}
			loaded = true;
      Repository.shared.LoadPackages();
			PackageProgressService.shared.Load();
      IllustrationService.shared.Load();
      VoiceSuite.LoadBySetting();
		}

		IEnumerator Start () {
			var resolution = Settings.ParseResolutino(Settings.GetString(Settings.RESOLUTION, "800x600"));
			Screen.SetResolution(resolution.x, resolution.y, Settings.IsFullscreen(), 60);
			Loading.Load();
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene("Main");
		}

	}

}
