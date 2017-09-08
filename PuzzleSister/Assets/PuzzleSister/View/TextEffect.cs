using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using System.IO;

namespace PuzzleSister {
	
  [RequireComponent(typeof(Text))]
  public class TextEffect : MonoBehaviour {

    public void SetText(string text) {
      GetComponent<Text>().text = text;
    }

  }

}