using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.UGCEditor {

  public class ItemAction : MonoBehaviour, IPointerClickHandler {

    public ActionType action;

    public void OnPointerClick(PointerEventData eventData) {
      GetComponentInParent<UGCEditor>().HandleAction(action, transform.GetComponentInParent<ItemView>().itemData);
    }

  }

}