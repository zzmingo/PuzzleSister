using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditorPackageList : MonoBehaviour {

    public GameObject prefabPackageRow;

    void Awake() {
      RefreshList();
      QEditorService.shared.OnPackageChange.AddListener(RefreshList);
    }

    void RefreshList() {
      var packageList = QEditorService.shared.GetAllPackages();
      var len = Math.Max(packageList.Count, transform.childCount);
      for(int i=0; i<len; i++) {
        if (i >= packageList.Count) {
          Destroy(transform.GetChild(i).gameObject);
        } else {
          Transform item;
          var package = packageList[i];
          if (i >= transform.childCount) {
            item = Instantiate(prefabPackageRow, Vector3.zero, Quaternion.identity, transform).transform;
          } else {
            item = transform.GetChild(i);
          }
          AdaptItem(item, package);
        }
      }
    }

    void AdaptItem(Transform item, QEditorService.PackageItem package) {
      item.name = package.id;
      item.Find("Name").GetComponent<Text>().text = package.name;
      item.Find("ImageCol/Image").GetComponent<Image>().sprite = SpriteExtensions.Base64ToSprite(package.image);
    }

  }

}