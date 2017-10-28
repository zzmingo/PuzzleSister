using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
	public class UIController : MonoBehaviour {

		[NotNull] public GameObject showingUIObject;
		[NotNull] public GameObject questionUIObject;
		[NotNull] public BGMController bGMController;
		[NotNull] public QuestionController questionController;

		private Stack<GameObject> uiStack = new Stack<GameObject>();

		void Awake() {
			GlobalEvent.shared.AddListener(handleGlobalEvent);
		}

		void handleGlobalEvent(EventData data) {
			switch(data.type) {
				case EventType.PackageItemClick:
					Package package = (data as PackageClickEventData).package;
          if (package == null) {
            Utils.ShowDLCStore();
          } else {
						PushUI(questionUIObject);
						bool chanllenge = PackageProgressService.shared.GetProgress(package.id).Completed;
						questionController.StartPackage(package, chanllenge);
						bGMController.RandomBGM();
          }
					break;
			}
		}

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

}
