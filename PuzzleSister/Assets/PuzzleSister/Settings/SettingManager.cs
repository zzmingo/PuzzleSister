using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour {

	public GameObject settingBtn;
	public GameObject soundBtn;
	public GameObject musicBtn;
	public GameObject screenBtn;
	public GameObject languageBtn;

    void Start () {
	}
	
	void Update () {
		
	}

	public void onSettingBtnClick() {
//		soundBtn.transform.position = settingBtn.transform.position;
//		musicBtn.transform.position = settingBtn.transform.position;
//		screenBtn.transform.position = settingBtn.transform.position;
//		languageBtn.transform.position = settingBtn.transform.position;
		soundBtn.SetActive(true);
		musicBtn.SetActive(true);
		screenBtn.SetActive(true);
		languageBtn.SetActive(true);
	}
}
