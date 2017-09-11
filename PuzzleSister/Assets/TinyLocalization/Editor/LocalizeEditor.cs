using UnityEngine;
using UnityEditor;

namespace TinyLocalization {
    [CustomEditor(typeof(Localize))]
    public class LocalizeEditor : Editor {
        bool advancedFoldoutOpen = false;
        
        public override void OnInspectorGUI()    {     
            Localize l = (Localize)target;
                        
            EditorGUILayout.Space();
        
            EditorGUILayout.BeginVertical(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.05f)));
            
            GUILayout.Label("Localize text", EditorStyles.largeLabel);
            
            if (LocalizationManager.Instance.Languages.Count == 0) {
                EditorGUILayout.HelpBox("Please add at least one language", MessageType.Error);
                EditorGUILayout.Separator();            
                EditorGUILayout.EndVertical();
                return;
            }
            
            EditorGUILayout.Separator();           
            
            EditorGUILayout.BeginVertical(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.2f)));            
                GUILayout.Label("Target key:", EditorStyles.boldLabel);
                l.TargetKey = EditorGUILayout.TextField(l.TargetKey);        
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Separator();
            
            if(l.TargetKey == "") {
                EditorGUILayout.HelpBox("Enter valid key", MessageType.Warning);
            } else if(!LocalizationManager.Instance.ContainsKey(l.TargetKey)) {
                if(GUILayout.Button("Add Key")) {
                    LocalizationManager.Instance.SetKey(l.TargetKey, "");
                }
            } else if(LocalizationManager.Instance.ContainsKey(l.TargetKey)) {
                GUILayout.Label("Localizations:", EditorStyles.boldLabel);
                
                EditorGUI.indentLevel++; 
                
                foreach (var language in LocalizationManager.Instance.Languages) {       
                    language.SetKey(l.TargetKey, EditorGUILayout.TextField(language.languageName, language.GetLocalizationKey(l.TargetKey).textValue)); 
                } 
                
                EditorGUI.indentLevel--; 
                
                EditorGUILayout.Space();
                
                EditorGUI.indentLevel++;
                advancedFoldoutOpen = EditorGUILayout.Foldout(advancedFoldoutOpen, "Advanced");
                if (advancedFoldoutOpen) {
                    EditorGUI.indentLevel++;
                    
                    l.Postfix = EditorGUILayout.TextField("Postfix", l.Postfix);
                                        
                    EditorGUI.indentLevel--; 
                }
                EditorGUI.indentLevel--;
                
                EditorGUILayout.Space();
                
                if(GUILayout.Button("Remove Key")) {
                    LocalizationManager.Instance.RemoveKey(l.TargetKey);
                }
            }
            
            EditorGUILayout.Separator();
            
            EditorGUILayout.EndVertical();            
        }
    }
}