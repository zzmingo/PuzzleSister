using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

namespace PuzzleSister {

  public class Repository {

    public static readonly Repository shared = new Repository();

    public readonly UnityEvent OnPackageChanged = new UnityEvent();
    public readonly UnityEvent OnPackagesLoaded = new UnityEvent();

    public bool IsPackagesLoaded { get { return isPackagesLoaded; } }

    private bool isPackagesLoaded = false;
    private List<Package> packageList = new List<Package>();

    public void LoadPackages() {
      if (isPackagesLoaded) {
        return;
      }
      bool internalTesting = false;
#if INTERNAL_TESTING
      internalTesting = true;
#endif
      if (internalTesting || Application.isEditor) {
        foreach(var pkgItem in DataConst.TESTING_PACKAGES) {
          Package pkg = new Package();
          var pkgCSVStr = CryptoUtils.Decript(Resources.Load<TextAsset>(pkgItem.packagePath).text);
          var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
          pkg.FromDict(pkgDict, pkgItem.questionPath);
          AddPackage(pkg);
        }
      }

      foreach(var pkgItem in DataConst.BUILTIN_PACKAGES) {
        Package pkg = new Package();
        var pkgCSVStr = CryptoUtils.Decript(Resources.Load<TextAsset>(pkgItem.packagePath).text);
        var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
        pkg.FromDict(pkgDict, pkgItem.questionPath);
        AddPackage(pkg);
      }

#if UNITY_STANDALONE
      // DLC
      // TODO check file broken
      // string appInstallDir = Utils.GetAppInstallDir();

      // if (Directory.Exists(appInstallDir)) {
      //   Debug.Log("Load DLC at " + appInstallDir);
      //   foreach(var path in Directory.GetDirectories(appInstallDir)) {
      //     if (File.Exists(Path.Combine(path, "Package.csv"))) {
      //       Debug.Log("Loading DLC");
      //       Package pkg = new Package();
      //       string pkgCSVStr = CryptoUtils.Decript(File.ReadAllText(Path.Combine(path, "Package.csv")));
      //       var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
      //       pkg.FromDict(pkgDict, Path.Combine(path, "Question.csv"), Package.Type.CSV, Package.Source.DLC);
      //       Debug.Log("  ID: " + pkg.id);
      //       AddPackage(pkg);
      //       Debug.Log("Loaded");
      //     }
      //   }
      // }
#endif

      isPackagesLoaded = true;
      OnPackagesLoaded.Invoke();
    }

    public Package[] GetAllPackages() {
      return packageList.ToArray();
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