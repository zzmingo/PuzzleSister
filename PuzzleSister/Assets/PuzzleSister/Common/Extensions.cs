using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public static class TextureExtensions {

  public static Texture2D Base64ToTexture(string base64) {
    if (string.IsNullOrEmpty(base64)) {
      return null;
    }
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
    var www = new WWW(path);
    Texture2D texture = www.texture;
    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) , 100f);
  }

  public static Sprite Base64ToSprite(string base64) {
    if (string.IsNullOrEmpty(base64)) {
      return null;
    }
    var texture = TextureExtensions.Base64ToTexture(base64);
    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) , 100f);
  }

  public static string ToBase64(this Sprite sprite) {
    return System.Convert.ToBase64String(sprite.texture.EncodeToPNG());
  }

  public static Sprite ToSprite(this string base64) {
    if (string.IsNullOrEmpty(base64)) {
      return null;
    }
    return Base64ToSprite(base64);
  }

  public static Sprite fromTexture(Texture2D texture) {
    return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f) , 100f);
  }

}

public static class ComponentQueryExtensions {

  public static T Query<T>(this GameObject gameObject, string path) {
    return gameObject.transform.Find(path).GetComponent<T>();
  }

  public static T Query<T>(this Transform transform, string path) {
    return transform.Find(path).GetComponent<T>();
  }

  public static T Query<T>(this MonoBehaviour monoBehaviour, string path) {
    return monoBehaviour.transform.Find(path).GetComponent<T>();
  }

}

public static class ArrayExtensions {

  public static T RandomOne<T>(this T[] array) {
    if (array.Length <= 0) {
      throw new UnityException("The array is empty");
    }
    if (array.Length == 1) {
      return array[0];
    }
    return array[UnityEngine.Random.Range(0, array.Length)];
  }

}

public static class StringExtensions {
	public static string ToUnicodeString(this string str) {
		StringBuilder strResult = new StringBuilder();
		if (!string.IsNullOrEmpty(str)) {
			for (int i = 0; i < str.Length; i++) {
				strResult.Append("\\u");
				strResult.Append(((int)str[i]).ToString("x"));
			}
		}
		return strResult.ToString();
	}

	public static string FromUnicodeString(this string str) {
		StringBuilder strResult = new StringBuilder();
		if (!string.IsNullOrEmpty(str)) {
			string[] strlist = str.Replace("\\", "").Split('u');
			try {
				for (int i = 1; i < strlist.Length; i++) {
					int charCode = Convert.ToInt32(strlist[i], 16);
					strResult.Append((char)charCode);
				}
			} catch (FormatException ex) {
				return Regex.Unescape(str);
			}
		}
		return strResult.ToString();
	}
}