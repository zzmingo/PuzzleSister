using UnityEngine;
using UnityEditor;

namespace TinyLocalization {
    [CustomEditor(typeof(Language))]
    public class LanguageEditor : Editor {       
        Vector2 scrollPos = Vector2.zero;
         
        public override void OnInspectorGUI()    {     
            Language l = (Language)target;
            
            EditorGUILayout.BeginVertical(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.05f)));
            
            EditorGUILayout.Space();
        
            EditorGUILayout.BeginVertical(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.1f)));
             
            GUILayout.Label("Language settings", EditorStyles.largeLabel);
            
            EditorGUILayout.Space();
            
            l.languageName = EditorGUILayout.TextField("Name", l.languageName);
            
            l.code = EditorGUILayout.TextField("Code", l.code);
            
            l.description = EditorGUILayout.TextField("Description", l.description);
            
            EditorGUILayout.EndVertical();
                        
            if(l.Keys.Count > 0) {            
                GUILayout.Label("Keys:", EditorStyles.boldLabel);
            
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false, GUILayout.Height(200));
                EditorGUI.indentLevel++;
                    foreach (var key in l.Keys) {
                        GUILayout.Space(5f);
                        EditorGUILayout.LabelField(key.key + ":", key.textValue);
                    }
                EditorGUI.indentLevel--;
                EditorGUILayout.EndScrollView();
            }  
            
            EditorGUILayout.EndVertical();
        }
    } 
}