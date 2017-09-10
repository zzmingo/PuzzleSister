using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace PuzzleSister {

  /// <summary>
  /// 回合答题服务
  /// </summary>
  public class PackageService {

    private static PackageService last;

    public static PackageService For(Package package) {
      if (last != null && last.package == package) {
        return last;
      }
      last = new PackageService(package);
      return last;
    }

    private Package package;
    private Dictionary<string, QuestionSaveData> packageSaveDict;
    private bool loaded;
    
    private PackageService(Package package) {
      this.package = package;
    }

    public void Save() {
      var packageSaveStr = JsonConvert.SerializeObject(packageSaveDict);
      Debug.Log(packageSaveStr);
      var saveDataStr = CryptoUtils.Encript(packageSaveStr);
      File.WriteAllText(GetSavePath(), saveDataStr);
    }

    public void Load() {
      if (loaded) {
        return;
      }
      var path = GetSavePath();
      if (!File.Exists(path)) {
        packageSaveDict = new Dictionary<string, QuestionSaveData>();
        return;
      }
      var saveDataStr = CryptoUtils.Decript(File.ReadAllText(path));
      Debug.Log("load\n" + saveDataStr);
      packageSaveDict = JsonConvert.DeserializeObject<Dictionary<string, QuestionSaveData>>(saveDataStr);
      loaded = true;
    }

    public bool IsComplete(Question question) {
      return packageSaveDict.ContainsKey(question.id) && packageSaveDict[question.id].completed;
    }

    public void SetCompleted(Question question) {
      if (!packageSaveDict.ContainsKey(question.id)) {
        packageSaveDict.Add(question.id, new QuestionSaveData());
      }
      packageSaveDict[question.id].completed = true;
    }


    public class QuestionSaveData {
      public bool completed;
    }

    private string GetSavePath() {
      string savesDir = Utils.Path(Utils.GetAppInstallDir(), Const.SAVE_DIR);
      if (!Directory.Exists(savesDir)) {
        Directory.CreateDirectory(savesDir);
      }
      return Utils.Path(Utils.GetAppInstallDir(), Const.SAVE_DIR, package.id + Const.SAVE_EXT);
    }

  }

}