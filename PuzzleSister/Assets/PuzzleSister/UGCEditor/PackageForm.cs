using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using PuzzleSister;
using TinyLocalization;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class PackageForm : MonoBehaviour {

		void Awake() {
			ResetFormData();
			UpdateFormUI();
		}

    public readonly PackageItem packageItem = new PackageItem();

    private bool blockingFormData = false;

    public void ResetFormData() {
      packageItem.name = null;
			packageItem.language = null;
      packageItem.description = null;
      packageItem.imagePath = null;
    }

    public void UpdateFormUI() {
      blockingFormData = true;
			this.Query<InputField>("Content/InputField-Name").text = packageItem.name;
			var dropdown = this.Query<Dropdown>("Content/Dropdown-Language");
			var languages = LocalizationManager.Instance.Languages;
			var options = new List<Dropdown.OptionData>();
			foreach (Language language in languages) {
				options.Add(new Dropdown.OptionData(language.languageName));
			}
			dropdown.options.Clear();
			dropdown.AddOptions(options);
			var value = languages.FindIndex (new Predicate<Language> (delegate(Language obj) {
				return obj.languageName.Equals(packageItem.language); 
			}));
			if (value < 0) {
				value = 0;
				packageItem.language = languages[0].code;
			}
			dropdown.value = value;
      this.Query<InputField>("Content/InputField-Description").text = packageItem.description;
      this.Query<ImageSelector>("Content/ImageArea").imagePath = packageItem.imagePath;
      this.Query<ImageSelector>("Content/ImageArea").UpdateImage();
      blockingFormData = false;
    }

    public void UpdateFormData() {
      if (blockingFormData) {
        return;
      }
			packageItem.name = this.Query<InputField>("Content/InputField-Name").text;
			var dropdown = this.Query<Dropdown>("Content/Dropdown-Language");
			packageItem.language = LocalizationManager.Instance.LanguageNameToCode(dropdown.options[dropdown.value].text);
      packageItem.description = this.Query<InputField>("Content/InputField-Description").text;
      packageItem.imagePath = this.Query<ImageSelector>("Content/ImageArea").imagePath;
    }

  }

}