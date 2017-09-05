using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {

  public static class Const {


    public static readonly string RESOURCES_PATH = "Assets/PuzzleSister/Resources/";
    public static readonly Dictionary<string, PackageItem> BUILTIN_PACKAGES = new Dictionary<string, PackageItem> {
      { "Assets/PuzzleSister/CSV/Test.csv", new PackageItem("Packages/Test", "Packages/Test/Package", "Packages/Test/Question", ".csv") }
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