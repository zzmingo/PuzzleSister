using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleSister.UGCEditor {

	public class QuestionCell : MonoBehaviour {
		
		void Update () {
			var rowSize = (transform.parent as RectTransform).sizeDelta;
			var elt = GetComponent<LayoutElement>();
			elt.minWidth = (rowSize.x - 80) / 9f - 1;
		}

	}
	
}
