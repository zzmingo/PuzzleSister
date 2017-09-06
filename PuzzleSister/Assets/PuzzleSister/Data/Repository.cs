using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Steamworks;
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
      bool internalTesting = false;
#if InternalTesting
      internalTesting = true;
#endif
      if (internalTesting || Application.isEditor) {
        foreach(var entry in DataConst.TESTING_PACKAGES) {
          Package pkg = new Package();
          var pkgCSVStr = Resources.Load<TextAsset>(entry.Value.packagePath).text;
          var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
          pkg.FromDict(pkgDict, entry.Value.questionPath);
          AddPackage(pkg);
        }
      }

      foreach(var entry in DataConst.BUILTIN_PACKAGES) {
        Package pkg = new Package();
        var pkgCSVStr = Resources.Load<TextAsset>(entry.Value.packagePath).text;
        var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
        pkg.FromDict(pkgDict, entry.Value.questionPath);
        AddPackage(pkg);
      }

      // DLC
      // TODO check file broken
      string appInstallDir = null;
      SteamApps.GetAppInstallDir(new AppId_t(Const.STEAM_APP_ID), out appInstallDir, 1024);
      Debug.Log("Load DLC at " + appInstallDir);
      foreach(var path in Directory.GetDirectories(appInstallDir)) {
        if (File.Exists(path + "/Package.csv")) {
          Debug.Log("Loading DLC");
          Package pkg = new Package();
          string pkgCSVStr = File.ReadAllText(path + "/Package.csv");
          var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
          pkg.FromDict(pkgDict, path + "/Question.csv", Package.Type.CSV, Package.Source.DLC);
          Debug.Log("  ID: " + pkg.id);
          AddPackage(pkg);
          Debug.Log("Loaded");
        }
      }
      

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