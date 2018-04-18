using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
    public class UIController : MonoBehaviour {

        public static UIController singleton;

        [NotNull] public GameObject uiMenu;
        [NotNull] public GameObject uiSettings;
        [NotNull] public GameObject showingUIObject;
        [NotNull] public GameObject packageListUIObject;
        [NotNull] public GameObject questionUIObject;
        [NotNull] public GameObject questionLangUIObject;
        [NotNull] public BGMController bGMController;
        [NotNull] public QuestionController questionController;

        private GameObject showingUIPopup;

        private Stack<GameObject> uiStack = new Stack<GameObject>();
        private Stack<GameObject> uiPopupStack = new Stack<GameObject>();

        void Awake() {
            singleton = this;
            GlobalEvent.shared.AddListener(handleGlobalEvent);
        }

        IEnumerator Start() {
            bGMController.PlayMenu();
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            yield return Loading.Load();
            showingUIObject.SetActive(true);
        }

        public void ToBuiltinPackages() {
            GameState.isShowBuiltins = true;
            PushPackageListUI();
        }

        public void ToUGCPackages() {
            GameState.isShowBuiltins = false;
            PushPackageListUI();
        }

        public void PushPackageListUI() {
            PushUI(packageListUIObject);
        }

        void handleGlobalEvent(EventData data) {
            switch (data.type) {
                case EventType.PackageItemClick:
                    Package package = (data as PackageClickEventData).package;
                    if (package == null) {
												Utils.ShowDLCStore();
                    } else {
                        PopPopup();
                        PushUI(questionUIObject);
                        bool chanllenge = PackageProgressService.shared.GetProgress(package.id).Completed;
                        questionController.StartPackage(package, chanllenge);
                        bGMController.PlayGame();
                    }
                    break;
                case EventType.CloseSettingsToMenu:
                    questionController.StopAndReset();
                    showingUIObject.SetActive(false);
                    uiStack.Clear();
                    showingUIObject = uiMenu;
                    uiMenu.SetActive(true);
                    bGMController.PlayMenu();
                    break;
                case EventType.QuestionPanelToPackageList:
                    bGMController.PlayMenu();
                    questionController.StopAndReset();
                    showingUIPopup = null;
                    uiPopupStack.Clear();
                    PopUI();
                    break;
                case EventType.CloseQuestionLangToSetting:
                    PopUI();
                    break;
            }
        }

        public void PushUI(GameObject uiGameObject) {
            Debug.Log("## push " + uiGameObject.name);
            uiGameObject.SetActive(true);
            showingUIObject.SetActive(false);
            uiStack.Push(showingUIObject);
            showingUIObject = uiGameObject;
        }

        public void PushPopup(GameObject uiPopupObject) {
            if (showingUIPopup != null)
            {
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
            Debug.Log("## push " + showingUIObject.name);
            showingUIObject.SetActive(false);
            showingUIObject = uiStack.Pop();
            showingUIObject.SetActive(true);
            if (showingUIObject == uiMenu) {
                bGMController.PlayMenu();
            }
        }

        public void PopPopup() {
            if (showingUIPopup != null)
            {
                showingUIPopup.SetActive(false);
                showingUIPopup = null;
                if (uiPopupStack.Count > 0)
                {
                    showingUIPopup = uiPopupStack.Pop();
                    showingUIPopup.SetActive(true);
                }
            }
        }

        void Update() {
			#if UNITY_STANDALONE
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonUp(1)) {

                // 正在答题不跳转
                if (showingUIObject != questionUIObject) {
                    PopUI();
                }
            }
			#endif
        }
    }
}
