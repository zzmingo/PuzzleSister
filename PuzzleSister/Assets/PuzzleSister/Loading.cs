using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleSister {

	public class Loading : MonoBehaviour {

		void Start () {
			var resolution = Settings.ParseResolutino(Settings.GetString(Settings.RESOLUTION, "800x600"));
			Screen.SetResolution(resolution.x, resolution.y, Settings.IsFullscreen(), 60);
			SceneManager.LoadScene("Main");
		}

	}

}
