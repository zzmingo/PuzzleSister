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
      Storage.shared.SerializeSave(GetSavePath(), packageSaveDict);
    }

    public void Load() {
      if (loaded) {
        return;
      }
      packageSaveDict = Storage.shared.DeserializeLoad(GetSavePath(), new Dictionary<string, QuestionSaveData>());
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
      return Utils.Path(package.id + Const.SAVE_EXT);
    }

  }

}