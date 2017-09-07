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

    public static PackageService For(Package package) {
      return new PackageService(package);
    }

    private Package package;
    private Dictionary<string, QuestionSaveData> packageSaveDict;
    
    private PackageService(Package package) {
      this.package = package;
    }

    public void Save() {
      var packageSaveStr = JsonConvert.SerializeObject(packageSaveDict);
      var saveDataStr = CryptoUtils.Encript(packageSaveStr);
      File.WriteAllText(GetSavePath(), saveDataStr);
    }

    public void Load() {
      var path = GetSavePath();
      if (!File.Exists(path)) {
        packageSaveDict = new Dictionary<string, QuestionSaveData>();
        return;
      }
      var saveDataStr = CryptoUtils.Decript(File.ReadAllText(path));
      packageSaveDict = JsonConvert.DeserializeObject<Dictionary<string, QuestionSaveData>>(saveDataStr);
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
      return Utils.Path(Utils.GetAppInstallDir(), Const.SAVE_DIR, package.id + Const.SAVE_EXT);
    }

  }

}