using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {

  public class MenuView : MonoBehaviour {

    private int focusIndex = 0;

    public void OnPointerEnterMenu(Transform tMenu) {
      FocusOn(tMenu);
    }

    public void FocusOn(Transform oMenu) {
      foreach(Transform tMenu in transform) {
        var cText = tMenu.Find("Text").GetComponent<Text>();
        cText.text = cText.text.Replace("〔  ", "").Replace("  〕", "");
        if (tMenu == oMenu.transform) {
          cText.text = "〔  " + cText.text + "  〕";
        }
      }
    }
  }

}