using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour {

	public GameObject settingBtn;
	public GameObject soundBtn;
	public GameObject musicBtn;
	public GameObject screenBtn;
	public GameObject languageBtn;
	private bool shown = false;
	private Vector3 settingBtnPos;
	private Vector3 soundBtnPos;
	private Vector3 musicBtnPos;
	private Vector3 screenBtnPos;
	private Vector3 languageBtnPos;

	void Start() {
		settingBtnPos = settingBtn.transform.position;
		soundBtnPos = soundBtn.transform.position;
		musicBtnPos = musicBtn.transform.position;
		screenBtnPos = screenBtn.transform.position;
		languageBtnPos = languageBtn.transform.position;
		soundBtn.transform.position = settingBtnPos;
		musicBtn.transform.position = settingBtnPos;
		screenBtn.transform.position = settingBtnPos;
		languageBtn.transform.position = settingBtnPos;
	}

	public void onSettingBtnClick() {
		Hashtable args = new Hashtable();
		shown = !shown;
		if (shown) {
			iTween.MoveTo(soundBtn, iTween.Hash (
				"position", soundBtnPos,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
			iTween.MoveTo(musicBtn, iTween.Hash(
				"position", musicBtnPos,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
			iTween.MoveTo(screenBtn, iTween.Hash(
				"position", screenBtnPos,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
			iTween.MoveTo(languageBtn, iTween.Hash(
				"position", languageBtnPos,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
		} else {
			iTween.MoveTo(soundBtn, iTween.Hash(
				"position", settingBtn.transform.position,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
			iTween.MoveTo(musicBtn, iTween.Hash(
				"position", settingBtn.transform.position,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
			iTween.MoveTo(screenBtn, iTween.Hash(
				"position", settingBtn.transform.position,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
			iTween.MoveTo(languageBtn, iTween.Hash(
				"position", settingBtn.transform.position,
				"easetype", iTween.EaseType.easeOutElastic,
				"time", 0.2f
			));
		}
	}
}
