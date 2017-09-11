using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System;

namespace PuzzleSister {

  [Serializable]
  public class IllustrationItem {
    public string id;
    public string name;
    public Sprite image;
    public bool rewarded;
  }

  /// <summary>
  /// 图鉴服务
  /// </summary>
  public class IllustrationService {

    public static readonly IllustrationService shared = new IllustrationService();

    private List<string> rewardedItemList;

    public void Load() {
      rewardedItemList = Storage.shared.DeserializeLoad(GetSavePath(), new List<string>());
    }

    public void Save() {
      Storage.shared.SerializeSave(GetSavePath(), rewardedItemList);
    }

    public void RewardItem(IllustrationItem item) {
      if (rewardedItemList.Contains(item.id)) {
        return;
      }
      rewardedItemList.Add(item.id);
    }

    private string GetSavePath() {
      return Utils.Path(Const.SAVE_PATH_ILLUSTRATION + Const.SAVE_EXT);
    }

  }

}