using UnityEngine;
using UnityEngine.UI;

namespace TinyLocalization {
    [ExecuteInEditMode]
    [RequireComponent(typeof(Text))]
    public class Localize : MonoBehaviour {
        [SerializeField] string targetKey = "";
        /// <summary>
        /// Localize target key
        /// </summary>
        public string TargetKey {
            get {
                return targetKey;
            }
            set {
                targetKey = value;
            }
        }
        
        /// <summary>
        /// Additional string after main translation value
        /// </summary>
        [SerializeField] string postfix = "";
        public string Postfix {
            get {
                return postfix;
            }
            set {
                postfix = value;
                
                UpdateLocalizeText();
            }
        }
        
        void OnEnable () {
            UpdateLocalizeText();
            
            LocalizationManager.OnChangeLanguage += OnChangeLanguage;
            Language.OnKeySet += OnKeySet;
        }
        
        void OnDisable () {
            LocalizationManager.OnChangeLanguage -= OnChangeLanguage;
            Language.OnKeySet -= OnKeySet;
        }
        
        void OnChangeLanguage(string languageCode){
            UpdateLocalizeText();
        }
        
        void OnKeySet(string languageCode, LocalizationKey changedKey){
            if (changedKey.key == targetKey)            
                UpdateLocalizeText();
        }
        
        public void UpdateLocalizeText(){
            GetComponent<Text>().text = LocalizationManager.Instance.GetLocalizedText(targetKey) + Postfix;
        }
    }
}