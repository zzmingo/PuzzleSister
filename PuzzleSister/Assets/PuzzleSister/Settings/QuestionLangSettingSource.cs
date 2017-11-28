using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {
	public class QuestionLangSettingSource : MonoBehaviour {
		void Awake() {
			var toggle = GetComponent<Toggle>();
			var text = transform.Find("Label").GetComponent<Text>().text;
			var questionLang = Settings.GetString(Settings.QUESTION_LANG, Settings.DEFAULT_QUESTION_LANG);
			toggle.isOn = questionLang.Contains(text);
			toggle.onValueChanged.AddListener((_) => {
				var langs = Settings.QuestionLangs();
				langs.Remove(text);
				if (toggle.isOn) {
					if (!langs.Contains(text)) {
						langs.Add(text);
					}
				} else {
					if (langs.Count > 1) {
						langs.Remove(text);
					} else {
						toggle.isOn = true;
					}
				}
				Settings.SaveQuestionLangs((string[])langs.ToArray(typeof(string)));
			});
		}
	}
}

