using UnityEngine;
using UnityEngine.Events;

namespace PuzzleSister {

  public enum EventType {
    Test
  }

  public class GlobalEvent : UnityEvent<EventData> {

    public static readonly GlobalEvent shared = new GlobalEvent();

    private GlobalEvent() {}

  }

  public class EventData {

    public EventType type;

  }

}