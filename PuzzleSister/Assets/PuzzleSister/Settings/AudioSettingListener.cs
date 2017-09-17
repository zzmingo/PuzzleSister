using UnityEngine;

namespace PuzzleSister {

  public class AudioSettingListener : MonoBehaviour {

    public Settings.Key key;

    void Awake() {
      var audioSource = GetComponent<AudioSource>();
      audioSource.volume = Settings.GetFloat(key.Strings(), 1);
      Settings.OnChange.AddListener((key) => {
        if (key != this.key.Strings()) return;
        audioSource.volume = Settings.GetFloat(key, 1);
      });
    }

  }

}