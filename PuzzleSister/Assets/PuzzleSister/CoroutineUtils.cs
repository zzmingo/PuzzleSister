using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleSister {

  public class CoroutineUtils : MonoBehaviour {

    public static Coroutine Start(IEnumerator enumerator) {
      var coroutineUtils = GameObject.FindGameObjectWithTag("CoroutineUtils");
      if (coroutineUtils == null) {
        coroutineUtils = new GameObject("CoroutineUtils");
        coroutineUtils.tag = "CoroutineUtils";
        coroutineUtils.AddComponent<CoroutineUtils>();
      }
      return coroutineUtils.GetComponent<CoroutineUtils>().StartCoroutine(enumerator);
    }

    public static void Stop(Coroutine coroutine) {
      var coroutineUtils = GameObject.FindGameObjectWithTag("CoroutineUtils");
      coroutineUtils.GetComponent<CoroutineUtils>().StopCoroutine(coroutine);
    }

  }

}