using System;
using UnityEngine;
using UnityEngine.UI;
using TinyLocalization;

namespace PuzzleSister {
	public class PackageLanguageSettingSource : MonoBehaviour {
		[NotNull] public GameObject template;
		void Awake() {
			var supporLangs = LocalizationManager.Instance.Languages;
			supporLangs.ForEach(new Action<Language>(delegate(Language language) {
				var obj = Instantiate(template, transform);
				obj.name = "LangToggle";
				obj.SetActive(true);
				var label = obj.transform.Find("Label").GetComponent<Text>();
				label.text = language.languageName;
				var code = language.code;
				var languageCodes = Settings.GetString(Settings.PACKAGE_LANGUAGE, Settings.SupportLanguageCodes());
				var toggle = obj.GetComponent<Toggle>();
				toggle.isOn = languageCodes.Contains(code);
				toggle.onValueChanged.AddListener((_) => {
					var codes = Settings.PackageLanguageCodes();
					if (toggle.isOn) {
						if (!codes.Contains(code)) {
							codes.Add(code);
							Settings.SavePackageLanguageCodes(codes);
						}
					} else {
						if (codes.Count > 1) {
							codes.Remove(code);
							Settings.SavePackageLanguageCodes(codes);
						} else {
							toggle.isOn = true;
						}
					}
				});
			}));
		}
	}
}

