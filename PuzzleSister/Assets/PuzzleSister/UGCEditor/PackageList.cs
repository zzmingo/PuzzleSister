using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using TinyLocalization;

namespace PuzzleSister.UGCEditor {

  public class PackageList : MonoBehaviour {

    public GameObject prefabPackageRow;

    private Coroutine coLoadPreview;

    public void InitList(List<PackageItem> packageList) {
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

    void AdaptItem(Transform item, PackageItem package) {
      item.Find("Name").GetComponent<Text>().text = package.name;
			var language = PackageLanguageSettingSource.SupprotLanguages[package.language];
      if (language == null) {
				language = PackageLanguageSettingSource.SupprotLanguages["zh_CN"];
      }
			item.Find("Language").GetComponent<Text>().text = language;
      item.Find("Description").GetComponent<Text>().text = package.description;
      item.GetComponent<ItemView>().itemData = package;
      if (coLoadPreview != null) {
        StopCoroutine(coLoadPreview);
      }
      coLoadPreview = StartCoroutine(LoadPreviewAsync(item, package));
    }

    IEnumerator LoadPreviewAsync(Transform item, PackageItem package) {
      WWW www = new WWW(package.imagePath);
      yield return www;
      Image image = item.Find("ImageCol/Image").GetComponent<Image>();
      if (www.texture != null) {
        image.sprite = Sprite.Create(
          www.texture, 
          new Rect(0, 0, www.texture.width, www.texture.height), 
          new Vector2(0.5f, 0.5f));
      } else {
        image.sprite = null;
      }
    }

  }

}