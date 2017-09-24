using UnityEngine;
using UnityEditor;
using System.IO;


public static class ScriptableObjectUtils {

  public static void CreateAsset<T> () where T : ScriptableObject {
    T asset = ScriptableObject.CreateInstance<T>();

    string path = AssetDatabase.GetAssetPath(Selection.activeObject);
    if (path == "")  {
      path = "Assets";
    } else if (Path.GetExtension (path) != "") {
      path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
    }

    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

    AssetDatabase.CreateAsset(asset, assetPathAndName);

    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
    EditorUtility.FocusProjectWindow();
    Selection.activeObject = asset;
  }

  [MenuItem("Puzzle Sisters/Create VoiceSuite")]
  public static void CreateAsset() {
    ScriptableObjectUtils.CreateAsset<PuzzleSister.VoiceSuite>();
  }

}