using UnityEngine;

namespace PuzzleSister {

  public class IllustrationSettings : MonoBehaviour {

    public static IllustrationSettings shared;

    public IllustrationItem[] items;

    void Awake() {
      shared = this;
    }
  }

}