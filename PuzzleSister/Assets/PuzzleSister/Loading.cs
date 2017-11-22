using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;
using PuzzleSister.UGCEditor;

namespace PuzzleSister {

	public class Loading : MonoBehaviour {

		private static bool loaded = false;

		public static IEnumerator Load() {
			if (loaded) {
				yield break;
			}

			EResult loadResult = EResult.k_EResultOK;
			yield return UGCService.shared.LoadSubscribed((result) => loadResult = result);

			Debug.Log("load packages");
			Repository.shared.LoadBuildtins();
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

		IEnumerator Start () {
			var resolution = Settings.ParseResolution(Settings.GetString(Settings.RESOLUTION, Settings.DEFAULT_RESOLUTION));
			Screen.SetResolution(resolution.width, resolution.height, Settings.IsFullscreen(), resolution.refreshRate);
			DontDestroyOnLoad(this);
			yield return Loading.Load();

			SceneManager.LoadScene("Main");

			transform.Find("Top").gameObject.MoveBy(new Vector3(0, 838, 0), 2f, 0.5f);
			transform.Find("Bottom").gameObject.MoveBy(new Vector3(0, -640, 0), 2f, 0.5f);

			yield return new WaitForSeconds(3f);
			Destroy(gameObject);

		}

	}

}
