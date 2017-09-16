using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
  
    void Awake() {
      Repository.shared.LoadPackages();
			PackageProgressService.shared.Load();

      oMenuView.SetActive(true);
      oPackageListView.SetActive(false);
      oQuestionPanel.SetActive(false);
      oSettingsView.SetActive(false);
      GlobalEvent.shared.AddListener(OnGlobalEvent);
      Repository.shared.LoadPackages();
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
          StartCoroutine(TransitionPackageListViewToQuestionPanel((data as PackageClickEventData).package));
          break;
        case EventType.QuestionPanelBackBtnClick:
        case EventType.QuestionPanelToPackageList:
          StartCoroutine(TransitionQuestionPanelToPackageListView());
          break;
        case EventType.OpenSettings:
          StartCoroutine(TransitionMenuToSettings());
          break;
        case EventType.CloseSettings:
          StartCoroutine(TransitionSettingsToMenu());
          break;
      }
    }

    IEnumerator TransitionMenuToSettings() {
      yield return HideMenu(0);
      yield return ShowSettings();
    }

    IEnumerator TransitionSettingsToMenu() {
      yield return HideSettings();
      yield return ShowMenu(0);
    }

    IEnumerator TransitionMenuToPackageListView() {
      blockingEvents = true;
      yield return HideMenu();
      yield return ShowPackageList();
      blockingEvents = false;
    }

    IEnumerator TransitionPackageListViewToMenu() {
      blockingEvents = true;
      yield return HidePackageList();
      yield return ShowMenu();
      blockingEvents = false;
    }

    IEnumerator TransitionPackageListViewToQuestionPanel(Package package) {
      yield return HidePackageList();
      oQuestionPanel.SetActive(true);
      oQuestionCharacter.ScaleFrom(new Vector3(0, 1f, 1f), 0.3f, 0);
      GetComponent<QuestionController>().StartPackage(package);
      bGMController.RandomBGM();
    }

    IEnumerator TransitionQuestionPanelToPackageListView() {
      oQuestionPanel.SetActive(false);
      GetComponent<QuestionController>().StopAndReset();
      yield return ShowPackageList();
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