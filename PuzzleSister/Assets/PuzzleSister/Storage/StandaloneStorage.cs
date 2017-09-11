#if UNITY_STANDALONE
using System.IO;

namespace PuzzleSister {

  public class StandaloneStorage : Storage {

    public override void Save(string path, string data, bool encript = true) {
      string savePath = GetSavePath(path);
      if (encript) {
        data = CryptoUtils.Encript(data);
      }
      File.WriteAllText(savePath, data);
    }
    public override string Load(string path, bool decript = true) {
      string savePath = GetSavePath(path);
      string data = File.Exists(savePath) ? File.ReadAllText(savePath) : null;
      if (decript && data != null) {
        data = CryptoUtils.Decript(data);
      }
      return data;
    }

    private string GetSavePath(string subpath) {
      string savePath = Utils.Path(Utils.GetAppInstallDir(), Const.SAVE_DIR, subpath);
      string saveDir = Path.GetDirectoryName(savePath);
      if (!Directory.Exists(saveDir)) {
        Directory.CreateDirectory(saveDir);
      }
      return savePath;
    }

  }

}

#endif