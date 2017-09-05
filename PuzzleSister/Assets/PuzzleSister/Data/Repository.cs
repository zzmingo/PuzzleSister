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
      bool internalTesting = false;
#if InternalTesting
      internalTesting = true;
#endif
      if (internalTesting || Application.isEditor) {
        foreach(var entry in DataConst.BUILTIN_PACKAGES) {
          Package pkg = new Package();
          var pkgCSVStr = Resources.Load<TextAsset>(entry.Value.packagePath).text;
          var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
          pkg.FromDict(pkgDict, entry.Value.questionPath);
          AddPackage(pkg);
        }
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