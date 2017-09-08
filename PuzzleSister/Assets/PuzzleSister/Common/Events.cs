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
  }

  public class GlobalEvent : UnityEvent<EventData> {

    public static readonly GlobalEvent shared = new GlobalEvent();

    private GlobalEvent() {}

  }

  public class EventData {

    public EventType type;

  }

  public class PackageClickEventData : EventData {
    public Package package;
  }

}