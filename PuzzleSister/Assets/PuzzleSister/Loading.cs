﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleSister {

	public class Loading : MonoBehaviour {

		private static bool loaded = false;

		public static void Load() {
			if (loaded) {
				return;
			}
			Debug.Log("load packages");
      Repository.shared.LoadPackages();
			Debug.Log("load PackageProgressService");
			PackageProgressService.shared.Load();
			Debug.Log("load IllustrationService");
      IllustrationService.shared.Load();
			Debug.Log("load VoiceSuite");
      VoiceSuite.LoadBySetting();
			Debug.Log("load ok");
			loaded = true;
		}

		void Start () {
			var resolution = Settings.ParseResolutino(Settings.GetString(Settings.RESOLUTION, "800x600"));
			Screen.SetResolution(resolution.x, resolution.y, Settings.IsFullscreen(), 60);
			Loading.Load();
			SceneManager.LoadScene("Main");
		}

	}

}
