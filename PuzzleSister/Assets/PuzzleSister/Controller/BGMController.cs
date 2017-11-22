using UnityEngine;

namespace PuzzleSister {

	public class BGMController : MonoBehaviour {

    [NotNull] public AudioClip[] clips;
    [NotNull] public AudioClip menuClip;
    [NotNull] public AudioClip editorClip;

    void Start() {
      RandomBGM();
    }

    public void PlayMenu() {
      var source = GetComponent<AudioSource>();
      source.clip = menuClip;
      source.Play();
      source.loop = true;
    }

    public void PlayEditor() {
      var source = GetComponent<AudioSource>();
      source.clip = editorClip;
      source.Play();
      source.loop = true;
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