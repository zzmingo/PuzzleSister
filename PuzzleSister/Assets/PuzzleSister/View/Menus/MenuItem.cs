using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {

  public class MenuItem : MonoBehaviour, IPointerEnterHandler {

    public void OnPointerEnter(PointerEventData eventData) {
      transform.parent.GetComponent<MenuView>().OnPointerEnterMenu(transform);
    }


  }

}