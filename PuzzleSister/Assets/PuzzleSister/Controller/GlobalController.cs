using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using System.IO;

namespace PuzzleSister {
	public class GlobalController : MonoBehaviour {

    public GameObject oMenuView;
    public GameObject oPackageListView;
    public GameObject oQuestionPanel;
  
    void Start() {

      oMenuView.SetActive(true);
      oPackageListView.SetActive(false);
      oQuestionPanel.SetActive(false);

      GlobalEvent.shared.AddListener(OnGlobalEvent);
      Repository.shared.LoadPackages();
    }
  
    void OnGlobalEvent(EventData data) {
      switch(data.type) {
        case EventType.MenuStartClick:
          TransitionMenuToPackageListView();
          break;
        case EventType.PackageListBackBtnClick:
          TransitionPackageListViewToMenu();
          break;
        case EventType.PackageItemClick:
          TransitionPackageListViewToQuestionPanel((data as PackageClickEventData).package);
          break;
      }
    }

    void TransitionMenuToPackageListView() {
      oMenuView.SetActive(false);
      oPackageListView.SetActive(true);
    }

    void TransitionPackageListViewToMenu() {
      oMenuView.SetActive(true);
      oPackageListView.SetActive(false);
    }

    void TransitionPackageListViewToQuestionPanel(Package package) {
      oPackageListView.SetActive(false);
      oQuestionPanel.SetActive(true);
      GetComponent<QuestionController>().StartPackage(package);
    }


  }

}