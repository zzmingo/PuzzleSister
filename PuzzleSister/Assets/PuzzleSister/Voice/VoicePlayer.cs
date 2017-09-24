using UnityEngine;

namespace PuzzleSister {

  [RequireComponent(typeof(AudioSource))]
  public class VoicePlayer : MonoBehaviour {

    public static VoicePlayer shared;

    void Awake() {
      shared = this;
    }

    void OnDestroy() {
      shared = null;
    }

    public void Play(AudioClip clip) {
      var source = GetComponent<AudioSource>();
      source.PlayOneShot(clip);
    }

  }

}