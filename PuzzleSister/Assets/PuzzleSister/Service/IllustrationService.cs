using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace PuzzleSister {

  public class IllustrationItem {
    public string id;
    public string name;
    public Sprite image;
  }

  /// <summary>
  /// 图鉴服务
  /// </summary>
  public class IllustrationService {

    public static readonly IllustrationService shared = new IllustrationService();

    private List<string> retivedItems;

    public void Load() {
      retivedItems = new List<string>();
      
    }

    public void Save() {
      
    }

    public void RetriveItem(IllustrationItem item) {
      
    }

  }

}