using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PuzzleSister {

  public class AudioSettingSource : MonoBehaviour {

    [NotNull] public Settings.Key key;
    public AudioClip audioClip;
    public float notifyTime = 1f;

    private Coroutine notifyCoroutine;

    void Awake() {
      var slider = GetComponent<Slider>();
      slider.value = Settings.GetFloat(key.Strings(), 1f);
      slider.onValueChanged.AddListener((value) => {
        Settings.SetFloat(key.Strings(), value);
        if (audioClip == null) {
          return;
        }
        if (notifyCoroutine != null) {
          StopCoroutine(notifyCoroutine);
        }
        notifyCoroutine = StartCoroutine(NotifyAudioVolume(value));
      });
    }

    IEnumerator NotifyAudioVolume(float volume) {
      GameObject gameObj = GameObject.FindGameObjectWithTag("NotifyAudioVolume");
      if (gameObj == null) {
        gameObj = new GameObject();
        gameObj.name = "AudioSettingSource.NotifyAudioVolume";
        gameObj.tag = "NotifyAudioVolume";
        gameObj.AddComponent<AudioSource>();
      }
      var audioSource = gameObj.GetComponent<AudioSource>();
      audioSource.clip = audioClip;
      audioSource.volume = volume;
      audioSource.Play();
      yield return new WaitForSeconds(notifyTime);
      audioSource.Stop();
      notifyCoroutine = null;
    }

  }

}