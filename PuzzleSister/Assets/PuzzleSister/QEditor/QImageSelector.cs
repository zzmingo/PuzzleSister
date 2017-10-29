using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using SFB;
using UnityEngine.Events;

namespace PuzzleSister.QEditor {

  public class QImageSelector : MonoBehaviour {

    public UnityEvent OnSelected;

    public String imagePath { get; private set; }
    public Image image;

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

  }

}