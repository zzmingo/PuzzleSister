using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace PuzzleSister {

  public static class DataConst {

    public static readonly string ENSCRIPT_KEY = "7ydsdgbp";

    public static readonly string RESOURCES_PATH = Utils.Path("Assets", "PuzzleSister", "Resources");

    public static readonly string PACKAGES_DIR = "Packages";
    public static readonly string PACKAGE_FILE_NAME = "Package";
    public static readonly string QUESTION_FILE_NAME = "Question";
    public static readonly string FILE_EXT = ".csv";

    public static readonly List<PackageItem> TESTING_PACKAGES = new List<PackageItem> {
    };

	public static readonly List<PackageItem> BUILTIN_PACKAGES = new List<PackageItem> {
		new PackageItem(
			Utils.Path(PACKAGES_DIR, "laosijishangluzhinan001"),
			Utils.Path(PACKAGES_DIR, "laosijishangluzhinan001", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "laosijishangluzhinan001", QUESTION_FILE_NAME),
			FILE_EXT
		),
    new PackageItem(
			Utils.Path(PACKAGES_DIR, "zishenwanjiazaijinhua001"),
			Utils.Path(PACKAGES_DIR, "zishenwanjiazaijinhua001", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "zishenwanjiazaijinhua001", QUESTION_FILE_NAME),
			FILE_EXT
		),
		new PackageItem(
			Utils.Path(PACKAGES_DIR, "zishenwanjiazaijinhua002"),
			Utils.Path(PACKAGES_DIR, "zishenwanjiazaijinhua002", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "zishenwanjiazaijinhua002", QUESTION_FILE_NAME),
			FILE_EXT
		),
		new PackageItem(
			Utils.Path(PACKAGES_DIR, "ribenmanhuajia001"),
			Utils.Path(PACKAGES_DIR, "ribenmanhuajia001", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "ribenmanhuajia001", QUESTION_FILE_NAME),
			FILE_EXT
		),
		new PackageItem(
			Utils.Path(PACKAGES_DIR, "quweilengzhishi001"),
			Utils.Path(PACKAGES_DIR, "quweilengzhishi001", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "quweilengzhishi001", QUESTION_FILE_NAME),
			FILE_EXT
		),
		new PackageItem(
			Utils.Path(PACKAGES_DIR, "dongwuquduoduo001"),
			Utils.Path(PACKAGES_DIR, "dongwuquduoduo001", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "dongwuquduoduo001", QUESTION_FILE_NAME),
			FILE_EXT
		),
		new PackageItem(
			Utils.Path(PACKAGES_DIR, "yinshijiankang001"),
			Utils.Path(PACKAGES_DIR, "yinshijiankang001", PACKAGE_FILE_NAME),
			Utils.Path(PACKAGES_DIR, "yinshijiankang001", QUESTION_FILE_NAME),
			FILE_EXT
		)
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