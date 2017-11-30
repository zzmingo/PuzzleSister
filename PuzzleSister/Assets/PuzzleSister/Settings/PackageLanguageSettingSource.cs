using System;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {
	public class PackageLanguageSettingSource : MonoBehaviour {
		[NotNull] public GameObject template;
		void Awake() {
			var supporQuestionLangs = Settings.SupportPackageLanguages ();
			supporQuestionLangs.ForEach(new Action<string>(delegate(string text) {
				var obj = Instantiate(template, transform);
				obj.name = "LangToggle";
				obj.SetActive(true);
				var label = obj.transform.Find("Label").GetComponent<Text>();
				label.text = text;
				var questionLang = Settings.GetString(Settings.PACKAGE_LANGUAGE, Settings.SUPPORT_PACKAGE_LANGUAGES);
				var toggle = obj.GetComponent<Toggle>();
				toggle.isOn = questionLang.Contains(text);
				toggle.onValueChanged.AddListener((_) => {
					var languages = Settings.PackageLanguages();
					if (toggle.isOn) {
						if (!languages.Contains(text)) {
							languages.Add(text);
							Settings.SavePackageLanguages(languages);
						}
					} else {
						if (languages.Count > 1) {
							languages.Remove(text);
							Settings.SavePackageLanguages(languages);
						} else {
							toggle.isOn = true;
						}
					}
				});
			}));
		}
	}
}

