using UnityEngine;

namespace PuzzleSister {

  public static class TextureExtensions {

    public static Texture2D Base64ToTexture(string base64) {
      Texture2D texture = new Texture2D(1, 1);
      FromBase64(texture, base64);
      return texture;
    }

    public static void FromBase64(this Texture2D texture, string base64) {
      byte[] bytes = System.Convert.FromBase64String(base64);
      texture.LoadImage(bytes);
    }

  }

  public static class SpriteExtensions {

    public static Sprite Base64ToSprite(string base64) {
      var texture = TextureExtensions.Base64ToTexture(base64);
      return Sprite.Create (texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) , 100f);
    }

  }

}