using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace PuzzleSister {

  public class PackageProgressService {

    public static readonly PackageProgressService shared = new PackageProgressService();

    private Dictionary<string, ProgressItem> progressDict;
    private bool loaded = false;

    public void Load() {
      if (loaded) return;
      progressDict = Storage.shared.DeserializeLoad(GetSavePath(), new Dictionary<string, ProgressItem>());
      Package[] packages = Repository.shared.GetAllPackages();
      foreach(var pkg in packages) {
        ProgressItem item;
        if (progressDict.ContainsKey(pkg.id)) {
          item = progressDict[pkg.id];
        } else {
          item = new ProgressItem();
          item.progress = 0;
          progressDict.Add(pkg.id, item);
        }
        item.total = pkg.CountQuestions();
      }
      loaded = true;
    }

    public void Save() {
      Storage.shared.SerializeSave(GetSavePath(), progressDict);
    }

    public ProgressItem GetProgress(string id) {
      return progressDict[id];
    }

    public void SetProgress(string id, int progress, int total) {
      if (!progressDict.ContainsKey(id)) {
        var item = new ProgressItem();
        item.progress = progress;
        item.total = total;
        progressDict.Add(id, item);
      } else {
        progressDict[id].progress = progress;
        progressDict[id].total = total;
      }
    }

    private string GetSavePath() {
      return Utils.Path(Const.SAVE_PATH_PACKAGE_PROGRESS + Const.SAVE_EXT);
    }

    public class ProgressItem {
      public int progress;
      public int total;

      public string Percentage(string flag = "%") {
        return Mathf.FloorToInt((progress + 0.0f) / total * 100) + flag;
      }
    }

  }

}