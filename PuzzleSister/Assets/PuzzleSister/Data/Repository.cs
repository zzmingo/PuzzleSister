using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
using Steamworks;
using PuzzleSister.UGCEditor;

namespace PuzzleSister {

  public class Repository {

    public static readonly Repository shared = new Repository();

    public readonly UnityEvent OnPackageChanged = new UnityEvent();
    public readonly UnityEvent OnPackagesLoaded = new UnityEvent();

    public bool IsPackagesLoaded { get { return isPackagesLoaded; } }

    private bool isPackagesLoaded = false;
    private List<Package> builtinList = new List<Package>();
    private List<Package> packageList = new List<Package>();

    public void LoadBuildtins() {
      builtinList.Clear();
      foreach(var pkgItem in DataConst.BUILTIN_PACKAGES) {
        Package pkg = new Package();
        var pkgCSVStr = CryptoUtils.Decript(Resources.Load<TextAsset>(pkgItem.packagePath).text);
        var pkgDict = CSVUtils.Parse(pkgCSVStr)[0];
        pkg.FromDict(pkgDict, pkgItem.questionPath);
        pkg.state = Package.State.Ready;
        builtinList.Add(pkg);
      }
    }

    public void LoadPackages() {
      List<PackageItem> packageList = UGCService.shared.GetAllPackages();
      this.packageList.Clear();
      foreach(PackageItem packageItem in packageList) {
        Package package = new Package();
        Debug.Log("Repository.loadPackages " + packageItem.name);
        package.id = packageItem.id;
        package.name = packageItem.name;
        package.description = packageItem.description;
        package.thumb = packageItem.imagePath;
        package.type = Package.Type.JSON;
        package.source = Package.Source.UGC;
        uint state = SteamUGC.GetItemState(packageItem.publishedFileId);
        bool downloaded = (state & (uint)EItemState.k_EItemStateInstalled) == (uint)EItemState.k_EItemStateInstalled;
        bool updateRequired = (state & (uint)EItemState.k_EItemStateNeedsUpdate) == (uint)EItemState.k_EItemStateNeedsUpdate;
        package.state = (downloaded && !updateRequired) ? Package.State.Ready : Package.State.Prepare;
        if (package.state == Package.State.Ready) {
          string folder = null;
          ulong size;
          uint timestamp;
          SteamUGC.GetItemInstallInfo(packageItem.publishedFileId, out size, out folder, 1024, out timestamp);
          package.path = folder;
        }
        this.packageList.Add(package);
      }
      OnPackagesLoaded.Invoke();
    }

    public Package[] GetBuiltinPackages() {
      return builtinList.ToArray();
    }

    public Package[] GetUGCPackages() {
      return packageList.ToArray();
    }

		public Package[] GetAllPackages() {
			Package[] allPackages = GetUGCPackages();
			GetBuiltinPackages().CopyTo(allPackages, 0);
			return allPackages;
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

    public void ReplacePackage(Package package) {
      var origin = GetPackageById(package.id);
      if (origin != null) {
        RemovePackage(origin);
      }
      AddPackage(package);
    }

  }

}