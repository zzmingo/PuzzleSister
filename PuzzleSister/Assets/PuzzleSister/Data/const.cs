using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {

  public static class DataConst {

    public static readonly string ENSCRIPT_KEY = "7ydsdgbp";

    public static readonly string RESOURCES_PATH = "Assets/PuzzleSister/Resources/";

    public static readonly List<PackageItem> TESTING_PACKAGES = new List<PackageItem> {
      new PackageItem("Packages/TEST0001", "Packages/TEST0001/Package", "Packages/TEST0001/Question", ".csv")
    };

    public static readonly List<PackageItem> BUILTIN_PACKAGES = new List<PackageItem> {
    };

    public class PackageItem {
      public readonly string folder;
      public readonly string ext;
      public readonly string packagePath;
      public readonly string questionPath;

      public PackageItem(string folder, string packagePath, string questionPath, string ext) {
        this.folder = folder;
        this.packagePath = packagePath;
        this.questionPath = questionPath;
        this.ext = ext;
      }
    }

  }

}