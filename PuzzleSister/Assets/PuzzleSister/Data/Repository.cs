using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PuzzleSister {

  public class Repository {

    public static readonly Repository shared = new Repository();

    public readonly UnityEvent OnPackageChanged = new UnityEvent();

    private List<Package> packageList = new List<Package>();

    public void LoadPackages() {
      if (Application.isEditor) {
        Package pkg = new Package();
        pkg.id = "Test0001";
        pkg.name = "测试包";
        pkg.path = "Test";
        pkg.type = Package.Type.CSV;
        pkg.source = Package.Source.Resources;
        AddPackage(pkg);
      }
    }

    public Package GetPackageById(string id) {
      return packageList.Find((pkg) => pkg.id.Equals(id));
    }

    public void AddPackage(Package package) {
      packageList.Add(package);
      this.OnPackageChanged.Invoke();
    }

    public void RemovePackage(Package package) {
      packageList.Remove(package);
      this.OnPackageChanged.Invoke();
    }

  }

}