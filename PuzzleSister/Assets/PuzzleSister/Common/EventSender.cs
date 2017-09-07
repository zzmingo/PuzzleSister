using UnityEngine;

namespace PuzzleSister {

  public class EventSender : MonoBehaviour {

    public EventType type;

    public void Send() {
      var data = new EventData();
      data.type = type;
      GlobalEvent.shared.Invoke(data);
    }

  }

}