using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace TinyLocalization {
[CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : Editor {
        int selectedTab = 0;
        
        int startLanguage = 0;
        
        Vector2 scrollPosition = Vector2.zero;
        
        string keyToAdd = "KEY_NAME";
        
        Dictionary<string, bool> keysFoldouts = new Dictionary<string, bool>();
        
        public override void OnInspectorGUI() {           
            LocalizationManager lm = (LocalizationManager)target;
            
            GUILayout.Label("LocalizationManager", EditorStyles.largeLabel);
            
            EditorGUILayout.Space();
            
            selectedTab = GUILayout.Toolbar (selectedTab, new string[] {"Languages", "Keys"});
            
            switch (selectedTab) {
                case 0:                
                    EditorGUILayout.Space();
                    
                    EditorGUILayout.BeginVertical(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.1f)));
                    
                    EditorGUILayout.Space();
                    
                    GUILayout.Label("Drag and drop language here", EditorStyles.wordWrappedLabel);
                    
                    if (lm.Languages.Count == 0) {
                        EditorGUILayout.HelpBox("No languages", MessageType.Warning);
                    }                    
                    
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition,  false, false, GUILayout.Height(100)); 
                        foreach (var language in lm.Languages) {
                            EditorGUILayout.Separator();
                                                        
                            EditorGUILayout.BeginHorizontal(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.2f)));
                            
                            if (GUILayout.Button("X", GUILayout.Height(18), GUILayout.Width(18)))   {
                                lm.RemoveLanguage(language);
                                break;
                            }
                                
                            EditorGUILayout.BeginVertical();
                            GUILayout.Space(4f);
                            EditorGUILayout.ObjectField(language, typeof (Language), false);
                            GUILayout.Space(2f);
                            EditorGUILayout.EndVertical();
                           
                            EditorGUILayout.EndHorizontal();
                        }
                    EditorGUILayout.EndScrollView();
                    
                    EditorGUILayout.EndVertical();                    
                    
                    CheckLanguagesDragable(GUILayoutUtility.GetLastRect (), lm); 
                                        
                    EditorGUILayout.Separator();
                    
                    GUILayout.Label("Start language", EditorStyles.boldLabel);
                    
                    List<string> languagesNames = new List<string>();
                    foreach (var language in lm.Languages) {
                        languagesNames.Add(language.name);
                    }                    
                    
                    startLanguage = 0;
                    int i = 0;    
                    foreach (var language in lm.Languages) {
                        if (language.code == lm.StartLanguage)
                            startLanguage = i;
                        i++;
                    } 
                        
                    int startLanguageNum = EditorGUILayout.Popup(startLanguage, languagesNames.ToArray());
                       
                    if(startLanguageNum < languagesNames.Count)
                        lm.StartLanguage = lm.Languages[startLanguageNum].code;        
                        
                    //"Start with device\nlanguage (if localized)"
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Start with device language (if localized)");
                    EditorGUILayout.Separator();
                    lm.StartWithDeviceLanguage = EditorGUILayout.Toggle(lm.StartWithDeviceLanguage);    
                    EditorGUILayout.EndHorizontal();        
                break;
                case 1:
                    if (lm.Languages.Count < 1)
                            return;
                        
                    var allKeys = lm.Languages[0].Keys;
                    
                    foreach (var key in allKeys) {
                        if (!keysFoldouts.ContainsKey(key.key))
                            keysFoldouts.Add(key.key, false);
                    }
                     
                    EditorGUILayout.Separator();
                    EditorGUILayout.BeginHorizontal(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.1f)));
                    
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, false, GUILayout.Height(300)); 
                    EditorGUI.indentLevel++;
                    
                        foreach (var key in allKeys) {  
                            EditorGUILayout.Separator(); 
                              
                            EditorGUILayout.BeginVertical(EditorHelper.MakeBackgroundStyle(new Color(0f, 0f, 0f, 0.2f)));     
                                 
                            EditorGUILayout.BeginHorizontal();                      
                            keysFoldouts[key.key] = EditorGUILayout.Foldout(keysFoldouts[key.key], key.key); 
                            
                            if(GUILayout.Button("Delete key")) {
                                lm.RemoveKey(key.key);
                                break;
                            }
                            EditorGUILayout.EndHorizontal();
                            
                            GUILayout.Space(6f);
                            
                            if(keysFoldouts[key.key]) {
                                EditorGUI.indentLevel++; 
                                                         
                                foreach (var language in lm.Languages) {         
                                    language.SetKey(key.key, EditorGUILayout.TextField(language.languageName, language.GetLocalizationKey(key.key).textValue));
                                } 
                                
                                EditorGUI.indentLevel--;
                                
                                EditorGUILayout.Separator();
                                
                                var newKeyName = EditorGUILayout.TextField("Rename key:", key.key);
                                
                                if (newKeyName != key.key) {
                                    lm.RenameKey(key.key, newKeyName);  
                                    keysFoldouts.Remove(key.key);
                                    if (!keysFoldouts.ContainsKey(newKeyName))
                                        keysFoldouts.Add(newKeyName, true);
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                        
                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.EndHorizontal();
                    
                    EditorGUILayout.Separator();
                    
                    keyToAdd = EditorGUILayout.TextField(keyToAdd);
                    
                    if (GUILayout.Button("Add new key"))   {
                        lm.SetKey(keyToAdd, "");
                        break;
                    }
                break;
            }
        }
        
        
        

        void CheckLanguagesDragable (Rect languageAddingRect, LocalizationManager lm) {
            if (Event.current.type == EventType.DragUpdated && languageAddingRect.Contains (Event.current.mousePosition)) {
                 foreach (Object obj in DragAndDrop.objectReferences)
                    if (obj.GetType() == typeof(Language))
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        
                 Event.current.Use ();
            }

            if (Event.current.type == EventType.DragPerform && languageAddingRect.Contains (Event.current.mousePosition)) {
                foreach (Object obj in DragAndDrop.objectReferences)
                    if (obj.GetType() == typeof(Language))
                        lm.AddLanguage((Language)obj);
            }
        } 
    }
}
