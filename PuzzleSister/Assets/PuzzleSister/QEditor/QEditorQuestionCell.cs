using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QEditorQuestionCell : MonoBehaviour {
	
	void Update () {
		var rowSize = (transform.parent as RectTransform).sizeDelta;
		var elt = GetComponent<LayoutElement>();
		elt.minWidth = (rowSize.x - 70) / 8f - 1;
	}

}
