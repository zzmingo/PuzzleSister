using System;
using UnityEngine;
using UnityEngine.UI;
using TinyLocalization;
using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {
	public class LanguageSettingSource : MonoBehaviour {

		void Awake() {
			var supporLanguages = LocalizationManager.Instance.Languages;
			var dropdown = GetComponent<Dropdown>();
			dropdown.options.Clear();
			List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
			var index = 0;
			var currentLanguage = Settings.GetString(Settings.LANGUAGE, Settings.DEFAULT_LANGUAGE);
			for (int i = 0, len = supporLanguages.Count; i < len; i++) {
				Language language = supporLanguages [i];
				options.Add(new Dropdown.OptionData(language.languageName));
				if (currentLanguage.Equals (language.code)) {
					index = i;
				}
			}
			dropdown.AddOptions(options);
			dropdown.value = index;
			dropdown.onValueChanged.AddListener ((int value) => {
				var code = LocalizationManager.Instance.LanguageNameToCode(dropdown.options[value].text);
				Settings.SetString(Settings.LANGUAGE, code);
				LocalizationManager.Instance.ChangeLanguage(code);
			});
		}
	}
}


