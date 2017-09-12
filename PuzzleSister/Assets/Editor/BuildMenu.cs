using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class BuildMenu {

    [MenuItem("Puzzle Sisters/Build All Platforms For Steam")]
    public static void BuildGame() {

      string[] levels = new string[] { "Assets/PuzzleSister/Asset/Main.unity" };
      string contentPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../Steam/content"));


      // Build player.
      BuildPipeline.BuildPlayer(
        levels, 
        Path.Combine(contentPath, "windows/PuzzleSisters.exe"), 
        BuildTarget.StandaloneWindows, 
        BuildOptions.Development | BuildOptions.AllowDebugging);
      BuildPipeline.BuildPlayer(
        levels, 
        Path.Combine(contentPath, "mac/PuzzleSisters.app"), 
        BuildTarget.StandaloneOSXIntel64, 
        BuildOptions.Development | BuildOptions.AllowDebugging);
    }
}