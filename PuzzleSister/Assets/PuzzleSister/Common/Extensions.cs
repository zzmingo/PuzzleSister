using UnityEngine;
using System;
using System.IO;

namespace PuzzleSister {

  public static class TextureExtensions {

    public static Texture2D Base64ToTexture(string base64) {
      Texture2D texture = new Texture2D(1, 1);
      texture.FromBase64(base64);
      return texture;
    }

    public static void FromBase64(this Texture2D texture, string base64) {
      byte[] bytes = System.Convert.FromBase64String(base64);
      texture.LoadImage(bytes);
    }

  }

  public static class SpriteExtensions {

    public static Sprite FromFile(string path) {
      path = Uri.UnescapeDataString(path).Substring(7);
      byte[] bytes = File.ReadAllBytes(path);
      Texture2D texture = new Texture2D(1, 1);
      texture.LoadImage(bytes);
      return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) , 100f);
    }

    public static Sprite Base64ToSprite(string base64) {
      var texture = TextureExtensions.Base64ToTexture(base64);
      return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) , 100f);
    }

    public static string ToBase64(this Sprite sprite) {
      return System.Convert.ToBase64String(sprite.texture.EncodeToPNG());
    }

  }

}