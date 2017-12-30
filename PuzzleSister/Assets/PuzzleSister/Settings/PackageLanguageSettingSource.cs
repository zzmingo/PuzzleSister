using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TinyLocalization;

namespace PuzzleSister {
	public class PackageLanguageSettingSource : MonoBehaviour {
		public static Dictionary<string, string> SupprotLanguages = new Dictionary<string, string>(){
			{"zh_CN", "简体中文"}, {"zh_TW", "繁体中文"}, {"en", "英语"},
			{"ja", "日语"}, {"ru", "俄语"}, {"de", "德语"}, {"pt", "葡萄牙语"},
			{"fr", "法语"}, {"it", "意大利语"}, {"ko", "朝鲜语"},
			{"es", "西班牙语"}, {"sq", "阿尔巴尼亚语"}, {"af", "南非荷兰语"},
			{"ar", "阿拉伯语"}, {"eu", "巴斯克语"}, {"bg", "保加利亚语"},
			{"be", "贝劳语"}, {"ca", "加泰罗尼亚语"}, {"hr", "克罗地亚语"},
			{"cs", "捷克语"}, {"da", "丹麦语"}, {"nl", "荷兰语"},
			{"et", "爱沙尼亚语"}, {"fo", "法罗语"}, {"fa", "波斯语"},
			{"fi", "芬兰语"}, {"gd", "盖尔语"}, {"in", "印度尼西亚语"}, 
			{"el", "希腊语"}, {"he", "希伯来语"}, {"is", "冰岛语"},
			{"hi", "北印度语"}, {"hu", "匈牙利语"}, {"rm", "拉丁语系"},
			{"lv", "拉脱维亚语"}, {"lt", "立陶宛语"}, {"mk", "马其顿语"},
			{"ms", "马来西亚语"}, {"mt", "马耳他语"}, {"no", "挪威语"},
			{"pl", "波兰语"}, {"ro", "罗马尼亚语"}, {"sz", "萨摩斯语"},
			{"sr", "塞尔维亚语"}, {"sk", "斯洛伐克语"}, {"zu", "祖鲁语"},
			{"sl", "斯洛文尼亚语"}, {"sb", "索布语"}, {"sx", "苏图语"},
			{"sv", "瑞典语"}, {"th", "泰语"}, {"ts", "汤加语"},
			{"tn", "瓦纳语"}, {"tr", "土耳其语"}, {"uk", "乌克兰语"},
			{"ur", "乌尔都语"}, {"ve", "文达语"}, {"vi", "越南语"},
			{"xh", "科萨语"}, {"ji", "依地语"}
		};

		[NotNull] public GameObject template;
		[NotNull] public GameObject content;
		void Awake() {
			foreach (var language in SupprotLanguages) {
				var obj = Instantiate(template, content.transform);
				obj.name = "LanguageToggle";
				obj.SetActive(true);
				var label = obj.transform.Find("Label").GetComponent<Text>();
				label.text = language.Value;
				var code = language.Key;
				var languageCodes = Settings.GetString(Settings.PACKAGE_LANGUAGE, Settings.SupportLanguageCodes());
				var toggle = obj.GetComponent<Toggle>();
				toggle.isOn = languageCodes.Contains(code);
				toggle.onValueChanged.AddListener ((_) => {
					var codes = Settings.PackageLanguageCodes ();
					if (toggle.isOn) {
						if (!codes.Contains (code)) {
							codes.Add (code);
							Settings.SavePackageLanguageCodes (codes);
						}
					} else {
						if (codes.Count > 1) {
							codes.Remove (code);
							Settings.SavePackageLanguageCodes (codes);
						} else {
							toggle.isOn = true;
						}
					}
				});
			}
		}
	}
}

