using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PuzzleSister {

  public static class DecriptUtils {
    
    public static string Descript(string message) {
      DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
      desProvider.Mode = CipherMode.CBC;
      desProvider.Padding = PaddingMode.Zeros;
      desProvider.Key = Encoding.ASCII.GetBytes(DataConst.ENSCRIPT_KEY);
      desProvider.IV = Encoding.ASCII.GetBytes(DataConst.ENSCRIPT_KEY);
      using (MemoryStream stream = new MemoryStream(Convert.FromBase64String(message))) {
        using (CryptoStream cs = new CryptoStream(stream, desProvider.CreateDecryptor(), CryptoStreamMode.Read)) {
          using (StreamReader sr = new StreamReader(cs, Encoding.UTF8)) {
            return sr.ReadToEnd();
          }
        }
      }
    }

  }

}