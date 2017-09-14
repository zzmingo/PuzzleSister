using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {
	public class GlobalController : MonoBehaviour {

    [NotNull] public GameObject oMenuView;
    [NotNull] public GameObject oPackageListView;
    [NotNull] public PackageListView cPackageListView;
    [NotNull] public GameObject oQuestionPanel;
    [NotNull] public GameObject oQuestionCharacter;
  
    void Start() {
      Repository.shared.LoadPackages();
			PackageProgressService.shared.Load();

      oMenuView.SetActive(true);
      oPackageListView.SetActive(false);
      oQuestionPanel.SetActive(false);

      GlobalEvent.shared.AddListener(OnGlobalEvent);
      Repository.shared.LoadPackages();
    }
  
    void OnGlobalEvent(EventData data) {
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
      }
    }

    IEnumerator TransitionMenuToPackageListView() {
      oMenuView.SetActive(false);
      yield return ShowPackageList();
    }

    IEnumerator TransitionPackageListViewToMenu() {
      yield return HidePackageList();
      oMenuView.SetActive(true);
    }

    IEnumerator TransitionPackageListViewToQuestionPanel(Package package) {
      yield return HidePackageList();
      oQuestionPanel.SetActive(true);
      oQuestionCharacter.ScaleFrom(new Vector3(0, 1f, 1f), 0.3f, 0);
      GetComponent<QuestionController>().StartPackage(package);
    }

    IEnumerator TransitionQuestionPanelToPackageListView() {
      oQuestionPanel.SetActive(false);
      GetComponent<QuestionController>().StopAndReset();
      yield return ShowPackageList();
    }

    IEnumerator ShowMenu() {
      yield return null;
    }

    IEnumerator ShowPackageList() {
      var oPanel = oPackageListView.transform.Find("Panel").gameObject;
      oPackageListView.SetActive(true);
      oPanel.transform.localScale = new Vector3(1f, 1f, 1f);
      oPanel.ScaleFrom(new Vector3(0, 1f, 1f), 0.4f, 0);
      yield return new WaitForSeconds(0.5f);
      cPackageListView.InitList();
    }

    IEnumerator HidePackageList() {
      var oPanel = oPackageListView.transform.Find("Panel").gameObject;
      oPanel.transform.localScale = new Vector3(1f, 1f, 1f);
      oPanel.gameObject.ScaleTo(new Vector3(1f, 0, 1f), 0.4f, 0);
      yield return new WaitForSeconds(0.4f);
      oPackageListView.SetActive(false);
      cPackageListView.DestroyList();
    }


  }

}