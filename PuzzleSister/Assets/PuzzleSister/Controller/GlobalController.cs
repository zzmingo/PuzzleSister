using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {
	public class GlobalController : MonoBehaviour {

    [NotNull] public BGMController bGMController;
    [NotNull] public GameObject oMenuView;
    [NotNull] public GameObject oPackageListView;
    [NotNull] public PackageListView cPackageListView;
    [NotNull] public GameObject oQuestionPanel;
    [NotNull] public GameObject oQuestionCharacter;
    [NotNull] public GameObject oSettingsView;

    private bool blockingEvents = false;
    private bool settingsOpening = false;
    private ViewType openingView = ViewType.None;

    public enum ViewType {
      None, Menu, PackageList, QuestionPanel,
    }
  
    void Awake() {
      Repository.shared.LoadPackages();
			PackageProgressService.shared.Load();

      oMenuView.SetActive(true);
      oPackageListView.SetActive(false);
      oQuestionPanel.SetActive(false);
      oSettingsView.SetActive(false);
      GlobalEvent.shared.AddListener(OnGlobalEvent);
      Repository.shared.LoadPackages();

      openingView = ViewType.Menu;
    }

    void Update() {
#if UNITY_STANDALONE
      if (!blockingEvents && Input.GetKeyUp(KeyCode.Escape)) {
        if (settingsOpening) {
          switch(openingView) {
            case ViewType.Menu:
              StartCoroutine(TransitionSettingsToMenu());
              break;
            case ViewType.QuestionPanel:
              StartCoroutine(TransitionSettingsToQuestionPanel());
              break;
          }
        } else {
          switch(openingView) {
            case ViewType.Menu:
              StartCoroutine(TransitionMenuToSettings());
              break;
            case ViewType.QuestionPanel:
              StartCoroutine(TransitionQuestionPanelToSettings());
              break;
          }
        }
      }
#endif
    }
  
    void OnGlobalEvent(EventData data) {
      if (blockingEvents) return;

      switch(data.type) {
        case EventType.MenuStartClick:
          StartCoroutine(TransitionMenuToPackageListView());
          break;
        case EventType.PackageListBackBtnClick:
          StartCoroutine(TransitionPackageListViewToMenu());
          break;
        case EventType.PackageItemClick:
          var package = (data as PackageClickEventData).package;
          if (package == null) {
            Utils.ShowDLCStore();
          } else {
            StartCoroutine(TransitionPackageListViewToQuestionPanel(package));
          }
          break;
        case EventType.QuestionPanelBackBtnClick:
        case EventType.QuestionPanelToPackageList:
          StartCoroutine(TransitionQuestionPanelToPackageListView());
          break;
        case EventType.OpenSettings:
          StartCoroutine(TransitionMenuToSettings());
          break;
        case EventType.CloseSettingsToMenu:
          StartCoroutine(TransitionQuestionPanelToMenu());
          break;
        case EventType.CloseSettings:
          switch(openingView) {
            case ViewType.Menu:
              StartCoroutine(TransitionSettingsToMenu());
              break;
            case ViewType.QuestionPanel:
              StartCoroutine(TransitionSettingsToQuestionPanel());
              break;
          }
          break;
      }
    }

    public void LoadQEditor() {
      SceneManager.LoadScene("QEditor");
    }

    IEnumerator TransitionMenuToSettings() {
      blockingEvents = true;
      yield return HideMenu(0);
      yield return ShowSettings();
      settingsOpening = true;
      blockingEvents = false;
    }

    IEnumerator TransitionSettingsToMenu() {
      blockingEvents = true;
      settingsOpening = false;
      yield return HideSettings();
      yield return ShowMenu(0);
      openingView = ViewType.Menu;
      blockingEvents = false;
    }

    IEnumerator TransitionMenuToPackageListView() {
      blockingEvents = true;
      yield return HideMenu();
      yield return ShowPackageList();
      openingView = ViewType.PackageList;
      blockingEvents = false;
    }

    IEnumerator TransitionPackageListViewToMenu() {
      blockingEvents = true;
      yield return HidePackageList();
      yield return ShowMenu();
      openingView = ViewType.Menu;
      blockingEvents = false;
    }

    IEnumerator TransitionPackageListViewToQuestionPanel(Package package) {
      blockingEvents = true;
      yield return HidePackageList();
      oQuestionPanel.SetActive(true);
      oQuestionCharacter.ScaleFrom(new Vector3(0, 1f, 1f), 0.3f, 0);
      GetComponent<QuestionController>().StartPackage(package);
      bGMController.RandomBGM();
      openingView = ViewType.QuestionPanel;
      blockingEvents = false;
    }

    IEnumerator TransitionQuestionPanelToPackageListView() {
      blockingEvents = true;
      oQuestionPanel.SetActive(false);
      GetComponent<QuestionController>().StopAndReset();
      yield return ShowPackageList();
      openingView = ViewType.PackageList;
      blockingEvents = false;
    }

    IEnumerator TransitionQuestionPanelToSettings() {
      blockingEvents = true;
      oQuestionPanel.SetActive(false);
      yield return ShowSettings();
      settingsOpening = true;
      blockingEvents = false;
    }

    IEnumerator TransitionSettingsToQuestionPanel() {
      blockingEvents = true;
      settingsOpening = false;
      yield return HideSettings();
      oQuestionPanel.SetActive(true);
      openingView = ViewType.QuestionPanel;
      blockingEvents = false;
    }

    IEnumerator TransitionQuestionPanelToMenu() {
      blockingEvents = true;
      settingsOpening = false;
      oQuestionPanel.SetActive(false);
      GetComponent<QuestionController>().StopAndReset();
      yield return HideSettings();
      yield return ShowMenu();
      openingView = ViewType.Menu;
      blockingEvents = false;
    }

    IEnumerator ShowMenu(float delay = 0.5f) {
      yield return new WaitForSeconds(delay);
      oMenuView.SetActive(true);
    }

    IEnumerator HideMenu(float delay = 0.5f) {
      yield return new WaitForSeconds(delay);
      oMenuView.SetActive(false);
    }

    IEnumerator ShowPackageList() {
      var oPanel = oPackageListView.transform.Find("Panel").gameObject;
      oPackageListView.SetActive(true);
      yield return new WaitForSeconds(0.5f);
      cPackageListView.InitList();
    }

    IEnumerator HidePackageList() {
      var oPanel = oPackageListView.transform.Find("Panel").gameObject;
      yield return new WaitForSeconds(0.4f);
      oPackageListView.SetActive(false);
      cPackageListView.DestroyList();
    }

    IEnumerator ShowSettings() {
      var oPanel = oSettingsView.transform.Find("Panel").gameObject;
      oPanel.transform.Find("Buttons/BtnToMenu").gameObject.SetActive(openingView == ViewType.QuestionPanel);
      oSettingsView.SetActive(true);
      oPanel.transform.localScale = new Vector3(1f, 1f, 1f);
      oPanel.ScaleFrom(new Vector3(0, 1f, 1f), 0.4f, 0);
      yield return new WaitForSeconds(0.5f);
    }

    IEnumerator HideSettings() {
      var oPanel = oSettingsView.transform.Find("Panel").gameObject;
      oPanel.transform.localScale = new Vector3(1f, 1f, 1f);
      oPanel.gameObject.ScaleTo(new Vector3(1f, 0, 1f), 0.4f, 0);
      yield return new WaitForSeconds(0.4f);
      oSettingsView.SetActive(false);
    }

  }

}