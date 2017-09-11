using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;

namespace TinyLocalization {
    [CreateAssetMenu(fileName = "Language", menuName = "TinyLocalization/Add Language", order = 291)]
    public class Language : ScriptableObject {
        public string languageName = "";	
        /// <summary>
        /// ISO 639-1 two-letter code
        /// </summary>
        public string code = "";	
        public string description = "";
        
        [SerializeField]
        List<LocalizationKey> keys = new List<LocalizationKey>();
        
        public List<LocalizationKey> Keys {
            get {
                return new List<LocalizationKey>(keys);
            }
        }
        
        public delegate void SetKeyAction(string languageCode, LocalizationKey changedKey);
        /// <summary>
        /// Event calls when key set. Only in editor mode
        /// </summary>
        public static event SetKeyAction OnKeySet;
        
        public void SetKey(string key, string textValue){
            LocalizationKey lk = GetLocalizationKey(key);
            
            if(lk == null)
                keys.Add(new LocalizationKey(key, textValue));
            else 
                lk.textValue = textValue;
                
            #if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            #endif
            
            if(!Application.isPlaying && OnKeySet != null) {
                OnKeySet(code, new LocalizationKey(key, textValue));
            }
        }
        
        public void RemoveKey(string key){
            LocalizationKey lk = GetLocalizationKey(key);
            
            if(lk != null)
                keys.Remove(lk);
            
            #if UNITY_EDITOR    
            EditorUtility.SetDirty(this);
            #endif
        }
        
        public string GetText(string key){
            LocalizationKey lk = GetLocalizationKey(key);
            
            if(lk != null)
                return lk.textValue;
            else
                return "";
        }
        
        public LocalizationKey GetLocalizationKey(string key){
            return keys.Find(k => k.key == key);
        }
        
        public bool ContainsKey(string key){
            if (keys.Find(lk => lk.key == key) != null)
                return true;
            
            return false;
        }
        
        
        public void Union(Language language) {
            foreach (var key in keys) {
                if (!language.ContainsKey(key.key)){
                    language.SetKey(key.key, "");
                }
            }
            foreach (var key in language.keys) {
                if (!ContainsKey(key.key)){
                    SetKey(key.key, "");
                }
            }
        }
    }
}