using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace PuzzleSister {

  public class IllustrationItem {
    public string name;
    public Sprite image;
  }

  /// <summary>
  /// 图鉴服务
  /// </summary>
  public class IllustrationService {

    public static readonly IllustrationService shared = new IllustrationService();

    private List<IllustrationItem> itemList;

    public void Load() {
      itemList = new List<IllustrationItem>();
      
    }

    public void RetriveItem() {

    }

  }

}