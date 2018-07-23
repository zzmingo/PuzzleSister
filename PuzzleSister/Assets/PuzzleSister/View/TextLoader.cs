using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {

  [RequireComponent(typeof(Text))]
  public class TextLoader : MonoBehaviour {

    public TextAsset zh_CNTextAsset;
		public TextAsset jaTextAsset;
		public TextAsset enTextAsset;
		public TextAsset zh_TWTextAsset;
		public TextAsset plTextAsset;

		private TinyLocalization.LocalizationManager.ChangeLanguageAction onChangeLanguage;

    public void Start() {
			onChangeLanguage = delegate {
				loadText();
			};
			TinyLocalization.LocalizationManager.OnChangeLanguage += onChangeLanguage;
			loadText();
    }

		private void loadText() {
			string code = TinyLocalization.LocalizationManager.Instance.CurrentLanguage.code;
			if (code == "zh-CN") {
				GetComponent<Text>().text = zh_CNTextAsset.text;
			} else if (code == "ja") {
				GetComponent<Text>().text = jaTextAsset.text;
			} else if (code == "en") {
				GetComponent<Text>().text = enTextAsset.text;
			} else if (code == "zh-TW") {
				GetComponent<Text>().text = zh_TWTextAsset.text;
			} else if (code == "pl") {
				GetComponent<Text>().text = plTextAsset.text;
			}
		}

		public void OnDestroy() {
			TinyLocalization.LocalizationManager.OnChangeLanguage -= onChangeLanguage;
		}
  }

}