using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
	public class UIController : MonoBehaviour {

		public static UIController singleton;

		[NotNull] public GameObject uiMenu;
		[NotNull] public GameObject uiSettings;
		[NotNull] public GameObject showingUIObject;
		[NotNull] public GameObject questionUIObject;
		[NotNull] public BGMController bGMController;
		[NotNull] public QuestionController questionController;

		private GameObject showingUIPopup;

		private Stack<GameObject> uiStack = new Stack<GameObject>();
		private Stack<GameObject> uiPopupStack = new Stack<GameObject>();

		void Awake() {
			singleton = this;
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
				case EventType.CloseSettingsToMenu:
					questionController.StopAndReset();
					showingUIObject.SetActive(false);
					uiStack.Clear();
					uiMenu.SetActive(true);
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

		public void PushPopup(GameObject uiPopupObject) {
			if (showingUIPopup != null) {
				showingUIPopup.SetActive(false);
				uiPopupStack.Push(showingUIPopup);
			}
			showingUIPopup = uiPopupObject;
			showingUIPopup.SetActive(true);
		}

		public void PopUI() {
			if (showingUIPopup != null) {
				PopPopup();
				return;
			}
			if (uiStack.Count <= 0) {
				return;
			}
			showingUIObject.SetActive(false);
			showingUIObject = uiStack.Pop();
			showingUIObject.SetActive(true);
		}

		public void PopPopup() {
			if (showingUIPopup != null) {
				showingUIPopup.SetActive(false);
				if (uiPopupStack.Count > 0) {
					showingUIPopup = uiPopupStack.Pop();
					showingUIPopup.SetActive(true);
				}
			}
		}

		void Update() {
	#if UNITY_STANDALONE
				if (Input.GetKeyUp(KeyCode.Escape)) {

					// 正在答题跳设置
					if (questionController.IsStartedPackage()) {
						PushUI(uiSettings);
					} 
					// 否则退去当前UI
					else {
						PopUI();
					}
				}

				if (Input.GetMouseButtonUp(1)) {
					// 右键在非答题状态下才启用
					if (!questionController.IsStartedPackage()) {
						PopUI();
					}
				}
	#endif
			}

	}

}
