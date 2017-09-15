using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister {

  public class AudioSettingSource : MonoBehaviour {

    public Settings.Key key;

    void Awake() {
      var slider = GetComponent<Slider>();
      slider.value = Settings.GetFloat(key.Strings(), 1f);
      slider.onValueChanged.AddListener((value) => {
        Debug.Log(value);
        Settings.SetFloat(key.Strings(), value);
      });
    }

  }

}