using UnityEngine;
using System;

namespace PuzzleSister {

  [RequireComponent(typeof(AudioSource))]
  public class PlayOnShotBugFix : MonoBehaviour {

    [Serializable]
    public class Item {
      public EventType type;
      public AudioClip clip;
    }

    public Item[] items;

    void Awake() {
      GlobalEvent.shared.AddListener((data) => {
        foreach(var item in items) {
          if (item.type == data.type) {
            GetComponent<AudioSource>().PlayOneShot(item.clip);
            break;
          }
        }
      });
    }

  }

}