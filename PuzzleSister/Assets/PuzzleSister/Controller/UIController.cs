using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

	private Stack<GameObject> uiStack = new Stack<GameObject>();
	public GameObject showingUIObject;

	void Start () {
		foreach(Transform child in transform) {
			child.gameObject.SetActive(false);
		}
		showingUIObject.SetActive(true);
	}

	public void PushUI(GameObject uiGameObject) {
		uiGameObject.SetActive(true);
		showingUIObject.SetActive(false);
		uiStack.Push(showingUIObject);
		showingUIObject = uiGameObject;
	}

	public void PopUI() {
		if (uiStack.Count <= 0) {
			return;
		}
		showingUIObject.SetActive(false);
		showingUIObject = uiStack.Pop();
		showingUIObject.SetActive(true);
	}

	void Update() {
#if UNITY_STANDALONE
      if (Input.GetKeyUp(KeyCode.Escape)) {
        PopUI();
      }
#endif
    }

}
