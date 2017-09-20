using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditorItemAction : MonoBehaviour, IPointerClickHandler {

    public QEditorAction aciton;

    public void OnPointerClick(PointerEventData eventData) {
      GetComponentInParent<QEditor>().OnPackageItemAction(transform.parent.parent.name, aciton);
    }

  }

}