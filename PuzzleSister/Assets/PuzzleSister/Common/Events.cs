using UnityEngine;
using UnityEngine.Events;

namespace PuzzleSister {

  public enum EventType {
    Test,
    MenuStartClick,
    MenuIllustrationClick,
    MenuSettingsClick,
    MenuManualClick,
    PackageItemClick,
    PackageListBackBtnClick,
    QuestionPanelBackBtnClick,
    DialogueConfirmed,
    SelectOptionA,
    SelectOptionB,
    SelectOptionC,
    SelectOptionD,
    QuestionPanelToPackageList,
    OpenSettings,
    CloseSettings,
    OpenManual,
    CloseManual,
    CloseSettingsToMenu,
    PlayStartAudio,
    PlayLiteHitAudio,
    OpenIllustration,
    CloseIllustration,
    PackageChanllengeClick,
    FullscreenEndingPlayEnd,
		CloseQuestionLangToSetting
    // 只能往后加
  }

  public class GlobalEvent : UnityEvent<EventData> {

    public static readonly GlobalEvent shared = new GlobalEvent();

    private GlobalEvent() {}

    public void Invoke(EventType type) {
      Debug.Log("Fire Event: " + type);
      var data = new EventData();
      data.type = type;
      Invoke(data);
    }
 
  }

  public class EventData {

    public EventType type;

  }

  public class PackageClickEventData : EventData {
    public Package package;
  }

}