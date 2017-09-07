using Steamworks;

namespace PuzzleSister {

  public static class Utils {

    public static string GetAppInstallDir() {
      string appInstallDir = null;
      SteamApps.GetAppInstallDir(new AppId_t(Const.STEAM_APP_ID), out appInstallDir, 1024);
      return appInstallDir;
    }
    public static string Path(params string[] paths) {
      return string.Join(System.IO.Path.DirectorySeparatorChar + "", paths);
    }

  }

}