using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PuzzleSister {

  public static class CryptoUtils {
    
    public static string Encript(string message) {
      using(DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider()) {
        desProvider.Mode = CipherMode.CBC;
        desProvider.Padding = PaddingMode.Zeros;
        desProvider.Key = Encoding.ASCII.GetBytes(DataConst.ENSCRIPT_KEY);
        desProvider.IV = Encoding.ASCII.GetBytes(DataConst.ENSCRIPT_KEY);
        using (var ms = new MemoryStream()) {
          using (var cs = new CryptoStream(ms, desProvider.CreateEncryptor(), CryptoStreamMode.Write)) {
            using (var sw = new StreamWriter(cs)) {
              sw.Write(message);
              sw.Flush();
              cs.FlushFinalBlock();
              sw.Flush();
              return Convert.ToBase64String(ms.ToArray());
            }
          }
        }
      }
    }

    public static string Decript(string message) {
      using(DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider()) {
        desProvider.Mode = CipherMode.CBC;
        desProvider.Padding = PaddingMode.Zeros;
        desProvider.Key = Encoding.ASCII.GetBytes(DataConst.ENSCRIPT_KEY);
        desProvider.IV = Encoding.ASCII.GetBytes(DataConst.ENSCRIPT_KEY);
        using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(message))) {
          using (CryptoStream cs = new CryptoStream(ms, desProvider.CreateDecryptor(), CryptoStreamMode.Read)) {
            using (StreamReader sr = new StreamReader(cs, Encoding.UTF8)) {
              return sr.ReadToEnd();
            }
          }
        }
      }
    }

  }

}