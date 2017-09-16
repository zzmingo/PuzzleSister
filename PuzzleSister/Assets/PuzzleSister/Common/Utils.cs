#if UNITY_STANDALONE
using Steamworks;
#endif
using UnityEngine;

namespace PuzzleSister {

  public static class Utils {

#if UNITY_STANDALONE
    public static string GetAppInstallDir() {
      string appInstallDir = null;
      SteamApps.GetAppInstallDir(new AppId_t(Const.STEAM_APP_ID), out appInstallDir, 1024);
      return appInstallDir;
    }
#endif

    public static string Path(params string[] paths) {
      return string.Join(System.IO.Path.DirectorySeparatorChar + "", paths);
    }

    public static void PlayClip(AudioClip clip) {
      GameObject
        .FindGameObjectWithTag("SoundEffect")
        .GetComponent<AudioSource>()
        .PlayOneShot(clip);
    }

  }

}