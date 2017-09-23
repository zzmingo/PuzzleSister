using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {

  [RequireComponent(typeof(Text))]
  public class ManualTextLoader : MonoBehaviour {

    public TextAsset manualTextAsset;

    public void Start() {
      GetComponent<Text>().text = manualTextAsset.text;
    }

  }

}