using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IMoveHandler {
	
	private static SettingManager instance;
	public static SettingManager Instance {
		get {
			if (instance == null) {
				return new GameObject("SettingManager").AddComponent<SettingManager>();
			} else {
				return instance;
			}
		}
	}

	private 

    void Start () {
		if (instance != null) {
			Destroy(gameObject);
			return;
		}

		instance = this;

		DontDestroyOnLoad(gameObject);
	}
	
	void Update () {
		
	}

	void OnDestroy() {
		if (instance != this) {
			return;
		}

		instance = null;
	}

    public void OnMove(AxisEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }
}
