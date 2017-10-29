using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SFB;
using UnityEngine.Events;

namespace PuzzleSister.UGCEditor {

  public class ImageSelector : MonoBehaviour {

    public UnityEvent OnSelected;
    [NotNull] public Image image;

    public String imagePath;

    public void UpdateImage() {
      if (!String.IsNullOrEmpty(imagePath)) {
        StartCoroutine(UpdateImageAsync());
      } else {
        image.sprite = null;
      }
    }

    public void OpenSelector() {
      var extensions = new [] {
        new ExtensionFilter("Image Files", "png", "jpg", "jpeg" )
      };
      var paths = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensions, false);
      if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0])) {
        var sprite = SpriteExtensions.FromFile(paths[0]);
        image.sprite = sprite;
        imagePath = paths[0];
        OnSelected.Invoke();
      }
    }

    IEnumerator UpdateImageAsync() {
      WWW www = new WWW(imagePath);
      yield return www;
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