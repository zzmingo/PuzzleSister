using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

namespace PuzzleSister {

	public class EventTest {

    [Test]
    public void TestSendAndListen() {
      bool received = false;
      GlobalEvent.shared.AddListener((data) => {
        received = true;
      });
      GameObject gameObj = new GameObject();
      var sender = gameObj.AddComponent<EventSender>();
      sender.type = EventType.Test;
      sender.Send();
      Assert.AreEqual(true, received);
    }
		
	}

}
