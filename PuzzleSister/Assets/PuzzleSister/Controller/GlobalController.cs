using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {

	public class GlobalController : MonoBehaviour {

    public void ExitGame() {
      Application.Quit();
    }

    public void GotoQEditor() {
      SceneManager.LoadScene("UGCEditor");
    }

  }

}