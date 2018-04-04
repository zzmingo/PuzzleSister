using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
	public class SettingManager : MonoBehaviour {

		public GameObject settingBtn;
		public GameObject soundBtn;
		public GameObject musicBtn;
		public GameObject screenBtn;
		public GameObject languageBtn;
		public GameObject resolutionBtn;
		public GameObject manualBtn;
		public GameObject closeBtn;
		public GameObject maskPanel;
		public GameObject bgPanel;
		public GameObject soundSlider;
		public GameObject musicSlider;
		public GameObject voiceSlider;
		private bool shown = false;
		private Vector3 settingBtnPos;
		private Vector3 soundBtnPos;
		private Vector3 musicBtnPos;
		private Vector3 screenBtnPos;
		private Vector3 languageBtnPos;
		private Vector3 resolutionBtnPos;
		private Vector3 manualBtnPos;
		private Vector3 closeBtnPos;

		void Start() {
			settingBtnPos = settingBtn.transform.position;
			soundBtnPos = soundBtn.transform.position;
			musicBtnPos = musicBtn.transform.position;
			screenBtnPos = screenBtn.transform.position;
			languageBtnPos = languageBtn.transform.position;
			resolutionBtnPos = resolutionBtn.transform.position;
			manualBtnPos = manualBtn.transform.position;
			closeBtnPos = closeBtn.transform.position;

			soundBtn.transform.position = settingBtnPos;
			musicBtn.transform.position = settingBtnPos;
			screenBtn.transform.position = settingBtnPos;
			languageBtn.transform.position = settingBtnPos;
			resolutionBtn.transform.position = settingBtnPos;
			manualBtn.transform.position = settingBtnPos;
			closeBtn.transform.position = settingBtnPos;
		}

		private void showOrHideBtns(bool show) {
			shown = show;
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
				iTween.MoveTo(resolutionBtn, iTween.Hash(
					"position", resolutionBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(manualBtn, iTween.Hash(
					"position", manualBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(closeBtn, iTween.Hash(
					"position", closeBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
			} else {
				iTween.MoveTo(soundBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(musicBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(screenBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(languageBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(resolutionBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(manualBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(closeBtn, iTween.Hash(
					"position", settingBtnPos,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
			}
		}

		public void onSettingBtnClick() {
			showOrHideBtns(!shown);
		}

		public void onMaskPanelClick() {
			this.maskPanel.SetActive(false);
		}

		public void onSoundBtnClick() {
			this.soundSlider.SetActive(true);
			this.musicSlider.SetActive(false);
			this.voiceSlider.SetActive(false);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onMusicBtnClick() {
			this.soundSlider.SetActive(false);
			this.musicSlider.SetActive(true);
			this.voiceSlider.SetActive(false);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onVoiceBtnClick() {
			this.soundSlider.SetActive(false);
			this.musicSlider.SetActive(false);
			this.voiceSlider.SetActive(true);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onCloseBtnClick() {
			showOrHideBtns(false);
			AlertUI.shared.Confirm("主人要休息了吗？", (bool result) => {
				if (result) {
					Application.Quit();
				}
			});
		}
	}

}