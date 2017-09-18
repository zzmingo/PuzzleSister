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

    private float showingTime = 0;
    private bool showing = false;
    private string target;

    public bool IsShowing() {
      return showing;
    }

    public void SetText(string text) {
      target = text;
      if (target == "") {
        GetComponent<Text>().text = text;
        return;
      }
      switch(type) {
        case Type.Sequence:
          showing = true;
          showingTime = 0;
          GetComponent<Text>().text = "";
          break;
        default: 
          GetComponent<Text>().text = text;
          break;
      }
    }

    public void ForceShowAll() {
      GetComponent<Text>().text = target;
      target = null;
      showing = false;
      showingTime = 0;
    }

    void Update() {
      if (showing) {
        showingTime += Time.deltaTime;
        if (showingTime >= time) {
          ForceShowAll();
        } else {
          var showCount = Mathf.FloorToInt(target.Length * showingTime / time);
          var showingText = target.Substring(0, showCount);
          GetComponent<Text>().text = showingText;
        }
      }
    }

  }

}