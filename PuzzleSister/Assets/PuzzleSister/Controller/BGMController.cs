using UnityEngine;

namespace PuzzleSister {

	public class BGMController : MonoBehaviour {

    [NotNull] public AudioClip[] clips;

    void Start() {
      RandomBGM();
    }

    public void RandomBGM() {
      int idx = Random.Range(0, clips.Length);
      var source = GetComponent<AudioSource>();
      source.clip = clips[idx];
      source.Play();
      source.loop = true;
    }

  }

}