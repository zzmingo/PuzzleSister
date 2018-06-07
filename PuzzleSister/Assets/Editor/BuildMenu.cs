using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class BuildMenu
{

    [MenuItem("Puzzle Sisters/Build All Platforms For Steam")]
    public static void BuildGame()
    {

        string[] levels = new string[] {
        "Assets/PuzzleSister/Asset/Loading.unity",
        "Assets/PuzzleSister/Asset/Main.unity",
        "Assets/PuzzleSister/Asset/UGCEditor.unity"
      };
        string contentPath = Path.GetFullPath(Path.Combine(Application.dataPath, "../../Steam/content"));


        // Build player.
        BuildPipeline.BuildPlayer(
          levels,
          Path.Combine(contentPath, "windows/PuzzleSisters.exe"),
          BuildTarget.StandaloneWindows, BuildOptions.None);
        BuildPipeline.BuildPlayer(
          levels,
          Path.Combine(contentPath, "mac/PuzzleSisters.app"),
          BuildTarget.StandaloneOSX, BuildOptions.None);
    }
}