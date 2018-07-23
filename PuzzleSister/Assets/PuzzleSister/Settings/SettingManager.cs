using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {
	public class SettingManager : MonoBehaviour {

		public GameObject settingBtn;
		public GameObject soundBtn;
		public GameObject musicBtn;
		public GameObject screenBtn;
		public GameObject voiceBtn;
		public GameObject resolutionBtn;
		public GameObject manualBtn;
		public GameObject closeBtn;
		public GameObject languageBtn;
		public GameObject maskPanel;
		public GameObject bgPanel;
		public GameObject soundSlider;
		public GameObject musicSlider;
		public GameObject voiceSlider;
		public GameObject resolution;

		public GameObject uiController;
		public GameObject manualUI;
		public Sprite cnImg;
		public Sprite jpImg;
		public Sprite enImg;
		public Sprite twImg;
		public Sprite plImg;

		private bool shown = false;

		void Start() {
			soundBtn.transform.position = musicBtn.transform.position = screenBtn.transform.position
				= voiceBtn.transform.position = resolutionBtn.transform.position = manualBtn.transform.position
				= closeBtn.transform.position = languageBtn.transform.position = settingBtn.transform.position;
			changeLanguageIcon(TinyLocalization.LocalizationManager.Instance.CurrentLanguage.code);
		}

		private void showOrHideBtns(bool show) {
			shown = show;
			if (shown) {
				iTween.MoveTo(languageBtn, iTween.Hash (
					"position", settingBtn.transform.TransformPoint(-624, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(soundBtn, iTween.Hash (
					"position", settingBtn.transform.TransformPoint(-546, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(musicBtn, iTween.Hash(
					"position", settingBtn.transform.TransformPoint(-468, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(screenBtn, iTween.Hash(
					"position", settingBtn.transform.TransformPoint(-312, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(voiceBtn, iTween.Hash(
					"position", settingBtn.transform.TransformPoint(-390, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(resolutionBtn, iTween.Hash(
					"position", settingBtn.transform.TransformPoint(-234, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(manualBtn, iTween.Hash(
					"position", settingBtn.transform.TransformPoint(-156, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(closeBtn, iTween.Hash(
					"position", settingBtn.transform.TransformPoint(-78, 0, 0),
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
			} else {
				iTween.MoveTo(languageBtn, iTween.Hash(
					"position", settingBtn.transform.position,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
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
				iTween.MoveTo(voiceBtn, iTween.Hash(
					"position", settingBtn.transform.position,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(resolutionBtn, iTween.Hash(
					"position", settingBtn.transform.position,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(manualBtn, iTween.Hash(
					"position", settingBtn.transform.position,
					"easetype", iTween.EaseType.easeOutElastic,
					"time", 0.2f
				));
				iTween.MoveTo(closeBtn, iTween.Hash(
					"position", settingBtn.transform.position,
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
			this.resolution.SetActive(false);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onMusicBtnClick() {
			this.soundSlider.SetActive(false);
			this.musicSlider.SetActive(true);
			this.voiceSlider.SetActive(false);
			this.resolution.SetActive(false);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onVoiceBtnClick() {
			this.soundSlider.SetActive(false);
			this.musicSlider.SetActive(false);
			this.voiceSlider.SetActive(true);
			this.resolution.SetActive(false);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onResolutionBtnClick() {
			this.soundSlider.SetActive(false);
			this.musicSlider.SetActive(false);
			this.voiceSlider.SetActive(false);
			this.resolution.SetActive(true);
			this.maskPanel.SetActive(true);
			showOrHideBtns(false);
		}

		public void onManualBtnClick() {
			showOrHideBtns(false);
			if (manualUI.activeSelf) {
				return;
			}
			uiController.GetComponent<UIController>().PushUI(manualUI);
		}

		public void onCloseBtnClick() {
			showOrHideBtns(false);
			AlertUI.shared.Confirm(TinyLocalization.LocalizationManager.Instance.GetLocalizedText("确认退出游戏？"), (bool result) => {
				if (result) {
					Application.Quit();
				}
			});
		}

		public void onLanguageBtnClick() {
			var manager = TinyLocalization.LocalizationManager.Instance;
			var currentLanguage = manager.CurrentLanguage;
			var languages = manager.Languages;
			for (int i = 0, len = languages.Count; i < len; i++) {
				if (languages[i].Equals(currentLanguage)) {
					var j = i + 1;
					if (j >= len) {
						j = 0;
					}
					var code = languages[j].code;
					manager.ChangeLanguage(code);
					changeLanguageIcon(code);
					break;
				}
			}
		}

		private void changeLanguageIcon(string code) {
			if (code == "zh-CN") {
				this.languageBtn.GetComponent<Image>().sprite = cnImg;
			} else if (code == "en") {
				this.languageBtn.GetComponent<Image>().sprite = enImg;
			} else if (code == "ja") {
				this.languageBtn.GetComponent<Image>().sprite = jpImg;
			} else if (code == "zh-TW") {
				this.languageBtn.GetComponent<Image>().sprite = twImg;
			} else if (code == "pl") {
				this.languageBtn.GetComponent<Image>().sprite = plImg;
			}
		}
	}

}