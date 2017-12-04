using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class PackageItem {

    public String id;
    public String name;
		public String language;
    public String description;
    public String imagePath;
    public uint timeUpdated;
    public bool visible;
    public PublishedFileId_t publishedFileId = PublishedFileId_t.Invalid;

    public void Set(PackageItem other) {
      id = other.id;
      name = other.name;
			language = other.language;
      description = other.description;
      imagePath = other.imagePath;
      publishedFileId = other.publishedFileId;
    }

    public PackageItem Clone() {
      PackageItem pkg = new PackageItem();
      pkg.id = id;
      pkg.name = name;
			pkg.language = language;
      pkg.description = description;
      pkg.imagePath = imagePath;
      pkg.publishedFileId = publishedFileId;
      return pkg;
    }

  }

}