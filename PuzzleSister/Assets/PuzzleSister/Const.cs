using UnityEngine;

namespace PuzzleSister {

  public static class Const {

    public const int STEAM_APP_ID = 710190;

    public const string QEDITOR_SAVE_DIR = "QEditorSavesDir";
    public const string QEDITOR_PACKAGES_FILE = "QEditorPackages.qeditor";

    public const string SAVE_DIR = "SavesDir";
    public const string SAVE_EXT = ".sav";

    public const string SAVE_DIR_PACAKGES = "Packages";
    public const string SAVE_PATH_PACKAGE_PROGRESS = "PackageProgress";
    public const string SAVE_PATH_ILLUSTRATION = "Illustraction";

    public const string COLOR_CORRECT_HEX_STRING = "#FFE500FF";
    public static readonly Color COLOR_CORRECT = new Color(0xFF/255.0f, 0xE5/255.0f, 0x00/255.0f, 1);

    public static readonly Vector2Int PACKAGE_IMAGE_SIZE = new Vector2Int(440, 270);

    public const int ILLUSTRATION_REWARD_BASE_FACTOR = 100;

  }

}