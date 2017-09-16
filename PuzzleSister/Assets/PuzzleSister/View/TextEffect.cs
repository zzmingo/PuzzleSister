using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {
	
  [RequireComponent(typeof(Text))]
  public class TextEffect : MonoBehaviour {

    public enum Type {
      None, Sequence
    }

    public Type type = Type.None;
    public float time = 1f;

    private Coroutine textCoroutine;
    private string target;

    public bool IsShowing() {
      return textCoroutine != null;
    }

    public void SetText(string text) {
      target = text;
      if (textCoroutine != null) {
        StopCoroutine(textCoroutine);
      }
      if (target == "") {
        GetComponent<Text>().text = text;
        return;
      }
      switch(type) {
        case Type.Sequence:
          GetComponent<Text>().text = "";
          textCoroutine = StartCoroutine(SequenceShowText(text));
          break;
        default: 
          GetComponent<Text>().text = text;
          break;
      }
    }

    public void ForceShowAll() {
      if (textCoroutine != null) {
        StopCoroutine(textCoroutine);
      }
      GetComponent<Text>().text = target;
    }

    IEnumerator SequenceShowText(string text) {
      var cText = GetComponent<Text>();
      var chars = text.ToCharArray();
      var totalLen = text.Length;
      var current = "";
      var index = 0;
      while(current.Length < totalLen) {
        yield return new WaitForSeconds(time/totalLen);
        current += chars[index];
        cText.text = current;
        index ++;
      }
      textCoroutine = null;
    }

  }

}