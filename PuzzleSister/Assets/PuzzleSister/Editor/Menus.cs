using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace PuzzleSister {

  public class Menus {

    [MenuItem("Puzzle Sisters/Transform CSV")]
    public static void TransformCSV() {
      foreach(var entry in DataConst.BUILTIN_PACKAGES) {
        var csvAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(entry.Key);
        var csvStr = csvAsset.text;
        var pkgCSVStr = CSVUtils.ExtractPackage(csvStr);
        var questionCSVStr = CSVUtils.ExtractQuestions(csvStr);
        var pkgPath = Application.dataPath + DataConst.RESOURCES_PATH.Substring("Assets".Length) + entry.Value.packagePath + entry.Value.ext;
        var questionPath = Application.dataPath + DataConst.RESOURCES_PATH.Substring("Assets".Length) + entry.Value.questionPath + entry.Value.ext;
        var folderPath = Application.dataPath + DataConst.RESOURCES_PATH.Substring("Assets".Length) + entry.Value.folder;
        if (Directory.Exists(folderPath)) {
          Directory.Delete(folderPath, true);
        }
        Directory.CreateDirectory(folderPath);
        File.WriteAllText(pkgPath, pkgCSVStr, Encoding.UTF8);
        File.WriteAllText(questionPath, questionCSVStr, Encoding.UTF8);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
      }
    } 

  }

}