using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditorService {

    public static readonly QEditorService shared = new QEditorService();

    public UnityEvent OnPackageChange = new UnityEvent();

    public bool packageDirty { get; private set; }

    private List<PackageItem> packageList;

    public void LoadPackages() {
      packageList = Storage.shared.DeserializeLoad(GetPackagesSavePath(), new List<PackageItem>());
    }

    public void SavePackages() {
      Storage.shared.SerializeSave(GetPackagesSavePath(), packageList);
    }

    public void AddPackage(PackageItem package) {
      package.id = Guid.NewGuid().ToString();
      packageList.Add(package);
      packageDirty = true;
      OnPackageChange.Invoke();
    }

    public PackageItem GetPackageById(string id) {
      return packageList.Find((item) => item.id.Equals(id));
    }

    public void RemovePackageById(string id) {
      var package = packageList.Find((item) => item.id.Equals(id));
      if (package == null) return;
      packageList.Remove(package);
      packageDirty = true;
      OnPackageChange.Invoke();
    }

    public void UpdatePackage(PackageItem package) {
      OnPackageChange.Invoke();
    }

    public List<PackageItem> GetAllPackages() {
      return packageList;
    }

    private string GetPackagesSavePath() {
      return Utils.Path(Const.QEDITOR_SAVE_DIR, Const.QEDITOR_PACKAGES_FILE);
    }

    public class PackageItem {
      public string id;
      public string name;
      public string image;
    }
    

  }

}