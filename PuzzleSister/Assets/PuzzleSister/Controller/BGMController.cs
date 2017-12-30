using UnityEngine;
using System.Collections;

namespace PuzzleSister {

	public class BGMController : MonoBehaviour {

    [NotNull] public AudioClip[] clips;
    [NotNull] public AudioClip menuClip;
    [NotNull] public AudioClip editorClip;

    public bool playingMenu = false;
    public bool playingEditor = false;
    public bool playingGame = false;

    private Coroutine fadeCoroutine;

    void Start() {
      PlayMenu();
    }

    public void PlayMenu() {
      if (playingMenu) {
        return;
      }
      playingMenu = true;
      playingEditor = false;
      playingGame = false;
      DoFadeTo(menuClip);
    }

    public void PlayEditor() {
      if (playingEditor) {
        return;
      }
      playingMenu = false;
      playingEditor = true;
      playingGame = false;
      DoFadeTo(editorClip);
    }

    public void PlayGame() {
      if (playingGame) {
        return;
      }
      playingMenu = false;
      playingEditor = false;
      playingGame = true;
      int idx = Random.Range(0, clips.Length);
      DoFadeTo(clips[idx]);
    }

    private void DoFadeTo(AudioClip clip) {
      if (fadeCoroutine != null) {
        StopCoroutine(fadeCoroutine);
        fadeCoroutine = null;
      }
      fadeCoroutine = StartCoroutine(FadeTo(clip));
    }

    // 渐变音效，用于切换时
    private IEnumerator FadeTo(AudioClip clip) {
      var source = GetComponent<AudioSource>();
      if (source.clip == null) {
        source.clip = clip;
        source.Play();
        source.loop = true;
      } else {
        float startVolume = source.volume;
        while(source.volume > 0) {
          source.volume -= startVolume * Time.deltaTime / 0.5f;
          yield return null;
        }
        source.clip = clip;
        source.loop = true;
        source.Play();
        float endVolume = Settings.GetFloat(Settings.Key.Music.Strings(), 1);
        while(source.volume < endVolume) {
          source.volume += endVolume * Time.deltaTime / 0.5f;
          if (source.volume > endVolume) {
            source.volume = endVolume;
            break;
          }
          yield return null;
        }
      }
    }
  }

}