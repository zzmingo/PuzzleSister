using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;

namespace TinyLocalization {
    //[CreateAssetMenu(fileName = "LocalizationManager", menuName = "TinyLocalization/Add LocalizationManager", order = 309)]
    //[ExecuteInEditMode]
    public class LocalizationManager : ScriptableObject {
        /// <summary>
        /// Path to LocalizationManager asset
        /// </summary>
        const string ASSET_PATH = "TinyLocalization";
        /// <summary>
        /// Key to save CurrentLanguage to PlayerPrefs
        /// </summary>
        const string PREFS_LANGUAGE_CURRENT = "settings.language.current";

        public delegate void ChangeLanguageAction(string languageCode);
        
        /// <summary>
        /// Call on language change
        /// </summary>
        public static event ChangeLanguageAction OnChangeLanguage;
            
        #region Languages
        [SerializeField] List<Language> languages = new List<Language>();
        /// <returns>Copy of list of languages</returns>
        public List<Language> Languages { 
            get {
                return new List<Language>(languages);
            }
        }
        
        [SerializeField] string startLanguage;
        /// <summary>
        /// Scene start with this language if CurrentLanguage not set
        /// </summary>
        public string StartLanguage { 
            get {
                if (StartWithDeviceLanguage) {
                    string languageCode = LanguageNameToCode(Application.systemLanguage.ToString());
                    
                    if (GetLanguage(languageCode) != null)
                        return languageCode;
                }
                
                return startLanguage;
            }
            set {
                startLanguage = value;
            }
        }
        
        /// <summary>
        /// Scene start with device language if CurrentLanguage not set
        /// </summary>
        public bool StartWithDeviceLanguage = false;
        
        /// <summary>
        /// Current language. Saved in PlayerPrefs
        /// </summary>
        public Language CurrentLanguage { 
            get {
                string languageCode = PlayerPrefs.GetString(PREFS_LANGUAGE_CURRENT);
                
                var language = GetLanguage(languageCode);
                
                if(language == null)
                    language = GetLanguage(StartLanguage);
                    
               return language;                    
            }
        }
        
        /// <summary>
        /// Delete CurrentLanguage save in PlayerPrefs
        /// </summary>
        public void CleanCurrentLanguage() {
            PlayerPrefs.DeleteKey(PREFS_LANGUAGE_CURRENT);
        }
        
        /// <summary>
        /// Add new language
        /// </summary>
        public void AddLanguage(Language language) {
            if (languages.Contains(language))
                return;
            
            languages.Add(language);
            SynsLanguages();
            
            if (languages.Count == 1)
                OnChangeLanguage(language.code);
            
            #if UNITY_EDITOR    
            EditorUtility.SetDirty(this);
            #endif
        }
        
        /// <summary>
        /// Remove language
        /// </summary>
        public void RemoveLanguage(Language language) {
            languages.Remove(language);
                
            #if UNITY_EDITOR     
            EditorUtility.SetDirty(this);
            #endif
        }
        
        /// <summary>
        /// Get language by two-letter code
        /// </summary>
        /// <param name="code">Two-letter code.</param>
        /// <returns>Language</returns>
        public Language GetLanguage(string code) {
            return Languages.Find(language => language.code == code);
        } 
        
        /// <summary>
        /// Synhronize all keys in all languages. If key not exist create ant set empty value
        /// </summary>
        public void SynsLanguages() {
            if(languages.Count < 2)
                return;
            
            for (int i = 0; i < languages.Count - 1; i++) {
                languages[i].Union(languages[i + 1]);
            }
        }
        
        /// <summary>
        /// Change current language
        /// </summary>
        /// <param name="code">Language two-letter code</param>
        public void ChangeLanguage(string code) {
            var language = GetLanguage(code);
            
            if (language == null) {
                Debug.LogError("TinyLocalization: Can't change language. Language not exist.");
                return;
            }
                        
            PlayerPrefs.SetString(PREFS_LANGUAGE_CURRENT, code);
            
            if (OnChangeLanguage != null)
                OnChangeLanguage(code);
        }
        
        #endregion
        
        
        
        #region Keys        
        /// <summary>
        /// Set key. Create new if not exist
        /// </summary>
        /// <param name="key">Localization key.</param>
        /// <param name="textValue">Localization text value.</param>
        public void SetKey(string key, string textValue) {
            foreach (var language in languages) {
                language.SetKey(key, textValue);
            }
        }
        
        public void RemoveKey(string key) {
            foreach (var language in languages) {
                language.RemoveKey(key);
            }
        }
        
        public bool ContainsKey(string key) {
            foreach (var language in languages)
                if (language.ContainsKey(key))
                    return true;
            
            return false;
        }
        
        public void RenameKey(string key, string newKey) {
            foreach (var language in languages) {
                var renameKey = language.GetLocalizationKey(key);
                
                if(renameKey == null)
                    continue;
                    
                if(language.ContainsKey(newKey))
                    continue;
                
                renameKey.key = newKey;
            }
        }
        
        /// <summary>
        /// Get localizet text depend on current language
        /// </summary>
        /// <param name="key">Localization key.</param>
        /// <returns>Localization text value.</returns>
        public string GetLocalizedText(string key) {
            if (CurrentLanguage == null)
                return "";
              
            return CurrentLanguage.GetText(key);
        } 
        
        #endregion
        
             
        /// <summary>
        /// Convert Unity system languages names to two-letter codes
        /// </summary>
        /// <param name="systemName">Unity system language name.</param>
        /// <returns>Two-letter code</returns>
        public string LanguageNameToCode(string systemName) {
            Dictionary<string, string> codesMap = new Dictionary<string, string> {
                {"Afrikaans", "af"}, {"Arabic", "ar"}, {"Basque", "eu"},
                {"Belarusian", "be"}, {"Bulgarian", "bg"}, {"Catalan", "ca"},
                {"简体中文", "zh-CN"}, {"Czech", "cs"}, {"Danish", "da"},
                {"Dutch", "nl"}, {"English", "en"}, {"Estonian", "et"},
                {"Faroese", "fo"}, {"Finnish", "fi"}, {"French", "fr"},
                {"German", "de"}, {"Greek", "el"}, {"Hebrew", "he"},
                {"Icelandic", "is"}, {"Indonesian", "id"}, {"Italian", "it"},
                {"日本语", "ja"}, {"Korean", "ko"}, {"Latvian", "lv"},
                {"Lithuanian", "lt"}, {"Norwegian", "no"}, {"Polish", "pl"},
                {"Portuguese", "pt"}, {"Romanian", "ro"}, {"Russian", "ru"},
                {"SerboCroatian", "sr"}, {"Slovak", "sk"}, {"Slovenian", "sl"},
                {"Spanish", "es"}, {"Swedish", "sv"}, {"Thai", "th"},
                {"Turkish", "tr"}, {"Ukrainian", "uk"}, {"Vietnamese", "vi"},
								{"Hungarian", "hu"}, {"繁体中文", "zh-TW"}
            };
            
            if (codesMap.ContainsKey(systemName))
                return codesMap[systemName];

            return "";
        }        
        
        #region Singleton
        
        #if UNITY_EDITOR
        [MenuItem ("Window/TinyLocalization/Select LocalizationManager", false, 80)]
        static void SelectLocalizationManager () {
            Selection.activeObject = Instance;
        }
        #endif
        
        
        /// <summary>
        /// Singleton
        /// </summary>
        /// <returns>LocalizationManager</returns>
        public static LocalizationManager Instance {
            get{
                if (instance == null) {
                    instance = Resources.Load(ASSET_PATH + "/" + "LocalizationManager", typeof(LocalizationManager)) as LocalizationManager;
                }
                
                if (instance == null) {
                    LocalizationManager asset = ScriptableObject.CreateInstance<LocalizationManager>();
                             
                    #if UNITY_EDITOR   
                    if(!AssetDatabase.IsValidFolder("Assets/Resources"))
                        AssetDatabase.CreateFolder("Assets", "Resources");
                    
                    if(!AssetDatabase.IsValidFolder("Assets/Resources/" + ASSET_PATH))    
                        AssetDatabase.CreateFolder("Assets/Resources", ASSET_PATH);        
                        
                    AssetDatabase.CreateAsset(asset, "Assets/Resources/" + ASSET_PATH + "/" + "LocalizationManager.asset");
                    AssetDatabase.SaveAssets();
                    #endif
                    
                    instance = asset;
                }
                
                Debug.Assert(instance != null);
                
                return instance;
            }
        }	
        private static LocalizationManager instance;
        #endregion
    }
}
