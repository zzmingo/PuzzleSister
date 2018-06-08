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

namespace PuzzleSister.UGCEditor
{

    public class PackageForm : MonoBehaviour
    {

        void OnEnable()
        {
            ResetFormData();
            UpdateFormUI();
        }

        public readonly PackageItem packageItem = new PackageItem();

        private bool blockingFormData = false;

        public void ResetFormData()
        {
            packageItem.name = null;
            packageItem.author = null;
            packageItem.language = null;
            packageItem.description = null;
            packageItem.imagePath = null;
        }

        public void UpdateFormUI()
        {
            blockingFormData = true;
            this.Query<InputField>("Content/InputField-Name").text = packageItem.name;
            this.Query<InputField>("Content/InputField-Author").text = packageItem.author;
            var dropdown = this.Query<Dropdown>("Content/Dropdown-Language");
            var options = new List<Dropdown.OptionData>();
            var codes = new List<string>(PackageLanguageSettingSource.SupprotLanguages.Keys);
            foreach (string code in codes)
            {
                options.Add(new Dropdown.OptionData(PackageLanguageSettingSource.SupprotLanguages[code]));
            }
            dropdown.options.Clear();
            dropdown.AddOptions(options);
            var value = codes.FindIndex(new Predicate<string>(delegate (string obj)
            {
                return obj.Equals(packageItem.language);
            }));
            if (value < 0)
            {
                value = 0;
                packageItem.language = codes[0];
            }
            dropdown.value = value;
            this.Query<InputField>("Content/InputField-Description").text = packageItem.description;
            this.Query<ImageSelector>("Content/ImageArea").imagePath = packageItem.imagePath;
            this.Query<ImageSelector>("Content/ImageArea").UpdateImage();
            blockingFormData = false;
        }

        public void UpdateFormData()
        {
            if (blockingFormData)
            {
                return;
            }
            packageItem.name = this.Query<InputField>("Content/InputField-Name").text;
            packageItem.author = this.Query<InputField>("Content/InputField-Author").text;
            var dropdown = this.Query<Dropdown>("Content/Dropdown-Language");
            foreach (var language in PackageLanguageSettingSource.SupprotLanguages)
            {
                if (language.Value.Equals(dropdown.options[dropdown.value].text))
                {
                    packageItem.language = language.Key;
                    break;
                }
            }
            packageItem.description = this.Query<InputField>("Content/InputField-Description").text;
            packageItem.imagePath = this.Query<ImageSelector>("Content/ImageArea").imagePath;
        }

    }

}