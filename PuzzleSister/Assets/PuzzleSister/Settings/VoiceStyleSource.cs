using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister {

  public class VoiceStyleSource : MonoBehaviour {

    void Awake() {
      var dropdown = GetComponent<Dropdown>();
      var styles = VoiceSuite.suites;
      var options = new List<Dropdown.OptionData>();
      foreach(var style in styles) {
        options.Add(new Dropdown.OptionData(style.name));
      }
      dropdown.options = options;

      var selected = Settings.GetString(Settings.VOICE_STYLE, styles[0].name);
      options = dropdown.options;
      for(int i=0; i<options.Count; i++) {
        var option = options[i];
        if (option.text.Equals(selected)) {
          dropdown.value = i;
          break;
        }
      }
      VoiceSuite.LoadBySetting();
      dropdown.onValueChanged.AddListener((int value) => {
        Settings.SetString(Settings.VOICE_STYLE, dropdown.options[value].text);
        VoiceSuite.LoadBySetting();
      });
    }

  }

}