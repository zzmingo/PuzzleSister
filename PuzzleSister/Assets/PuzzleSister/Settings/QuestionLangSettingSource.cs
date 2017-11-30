using System;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {
	public class QuestionLangSettingSource : MonoBehaviour {
		[NotNull] public GameObject template;
		void Awake() {
			var supporQuestionLangs = Settings.SupportQuestionLangs ();
			supporQuestionLangs.ForEach(new Action<string>(delegate(string text) {
				var obj = Instantiate(template, transform);
				obj.name = "LangToggle";
				obj.SetActive(true);
				var label = obj.transform.Find("Label").GetComponent<Text>();
				label.text = text;
				var questionLang = Settings.GetString(Settings.QUESTION_LANG, Settings.DEFAULT_QUESTION_LANG);
				var toggle = obj.GetComponent<Toggle>();
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
					Settings.SaveQuestionLangs(langs);
				});
			}));
		}
	}
}

