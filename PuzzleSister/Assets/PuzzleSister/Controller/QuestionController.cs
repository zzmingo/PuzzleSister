using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using System.IO;

namespace PuzzleSister {
	public class QuestionController : MonoBehaviour {

    public TextEffect cPackageTitle;
    public TextEffect cEnergy;
    public TextEffect cProgress;
    public TextEffect cTitle;
    public TextEffect cDialogue;
    public GameObject oDialogue;
    public GameObject oOptions;
    

    public void StartPackage(Package package) {
      cTitle.SetText("");
      cDialogue.SetText("");
    }

  }

}