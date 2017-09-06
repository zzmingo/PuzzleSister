using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

namespace PuzzleSister {
	public class GameController : MonoBehaviour {
	
	public GameObject oStartScreen;
	public GameObject oPackagePanel;
	public GameObject oQuestionPanel;
	public GameObject oQuestionCT;
	public PackageListView cPackageListView;
	public Text cHeartText;
	public Text cProgressText;

	private int totalCount = 0;
	private int heartCount = 5;
	private int answerCount = 0;
	private Coroutine questionCoroutine;
	private Question.Result result = Question.Result.Unknow;
	private List<Question> questionList;

	void Start() {
		Repository.shared.LoadPackages();
		oStartScreen.SetActive(true);
		oPackagePanel.SetActive(false);
		oQuestionPanel.SetActive(false);
		
		cPackageListView.OnItemClick.AddListener(OnClickPackageItem);
	}

	public void OnTapBtnStart() {
		oStartScreen.SetActive(false);
		oPackagePanel.SetActive(true);
		oQuestionPanel.SetActive(false);
	}

	public void OnClickPackageItem(Package package) {
		if (package == null) {
			SteamFriends.ActivateGameOverlayToStore(new AppId_t(Const.STEAM_APP_ID), EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
		} else {
			oStartScreen.SetActive(false);
			oPackagePanel.SetActive(false);
			oQuestionPanel.SetActive(true);
			StartQuestionGame(package);
		}
	}

	public void BackFromPackagePanel() {
		oStartScreen.SetActive(true);
		oPackagePanel.SetActive(false);
		oQuestionPanel.SetActive(false);
	}

	public void BackFromQuestionPanel() {
		if (questionCoroutine != null) {
			StopCoroutine(questionCoroutine);
		}
		oStartScreen.SetActive(false);
		oPackagePanel.SetActive(true);
		oQuestionPanel.SetActive(false);
	}

	public void OnSelectA() {
		if (result != Question.Result.Unknow) {
			return;
		}
		result = Question.Result.A;
	}

	public void OnSelectB() {
		if (result != Question.Result.Unknow) {
			return;
		}
		result = Question.Result.B;
	}

	public void OnSelectC() {
		if (result != Question.Result.Unknow) {
			return;
		}
		result = Question.Result.C;
	}

	public void OnSelectD() {
		if (result != Question.Result.Unknow) {
			return;
		}
		result = Question.Result.D;
	}

	void StartQuestionGame(Package package) {
		questionList = package.Load();
		totalCount = questionList.Count;
		heartCount = 5;
		if (questionCoroutine != null) {
			StopCoroutine(questionCoroutine);
		}
		UpdateHeartAndProgress();
		questionCoroutine = StartCoroutine(NextQuestion());
	}

	void UpdateHeartAndProgress() {
		cHeartText.text = "星：" + heartCount + "个";
		cProgressText.text = "进度：" + (1+totalCount-questionList.Count) + "/" + totalCount;
	}

	IEnumerator NextQuestion() {
		if (questionList.Count > 0) {
			yield return HandleNextQuestion();
		}
		BackFromQuestionPanel();
	}

	IEnumerator HandleNextQuestion() {
		UpdateHeartAndProgress();

		int idx = Random.Range(0, questionList.Count);
		var question = questionList[idx];

		var oQuestion = oQuestionCT.transform.Find("Question").gameObject;
		var oExplain = oQuestionCT.transform.Find("Explain").gameObject;
		var oOptions = oQuestionCT.transform.Find("Options").gameObject;

		oExplain.SetActive(false);

		// title
		int no = totalCount - questionList.Count;
		oQuestion.GetComponent<Text>().text = no + ". " + question.title;

		// options
		oOptions.SetActive(true);
		string[] options = new string[] { "OptionA", "OptionB", "OptionC", "OptionD" };
		foreach(var optName in options) {
			var oOption = oQuestionCT.transform.Find("Options/" + optName).gameObject;
			var cOptionText = oOption.transform.Find("Text").GetComponent<Text>();
			oOption.GetComponent<Image>().color = Color.white;
			oOption.GetComponent<Button>().interactable = true;
			switch(optName) {
				case "OptionA": 
					oOption.SetActive(question.optionA != null);
					cOptionText.text = "A. " + question.optionA;
					break;
				case "OptionB":
					oOption.SetActive(question.optionB != null);
					cOptionText.text = "B. " + question.optionB;
					break;
				case "OptionC":
					oOption.SetActive(question.optionC != null);
					cOptionText.text = "C. " + question.optionC;
					break;
				case "OptionD":
					oOption.SetActive(question.optionD != null);
					cOptionText.text = "D. " + question.optionD;
					break;
			}
		}

		result = Question.Result.Unknow;
		answerCount = 0;

		bool answerCorrect = false;

		while(answerCount < 3 && !answerCorrect) {
			while(result == Question.Result.Unknow) {
				yield return 1;
			}
			Debug.Log(question.result);
			Debug.Log(result);

			bool correct = result == question.result;

			// resolving heart
			if (result == question.result) {
				heartCount ++;
				if (heartCount > 5) {
					heartCount = 5;
				}
			} else {
				heartCount --;
				if (heartCount < 0) {
					heartCount = 0;
				}
			}
			UpdateHeartAndProgress();

			if (correct) {
				answerCorrect = true;
				break;
			}

			// warning incorrect option
			GameObject oOption = null;
			switch(result) {
				case Question.Result.A: oOption = oOptions.transform.Find("OptionA").gameObject; break;
				case Question.Result.B: oOption = oOptions.transform.Find("OptionB").gameObject; break;
				case Question.Result.C: oOption = oOptions.transform.Find("OptionC").gameObject; break;
				case Question.Result.D: oOption = oOptions.transform.Find("OptionD").gameObject; break;
			}
			oOption.GetComponent<Button>().interactable = false;
			oOption.GetComponent<Image>().color = Color.red;

			// reset result
			result = Question.Result.Unknow;
			answerCount ++;
		}

		
		// explaination
		oExplain.SetActive(false);
		var cExplainText = oExplain.GetComponent<Text>();
		var resultText = result == question.result ? "回答正确" : "回答错误";
		cExplainText.text = resultText + "\r\n\r\n" + 
			(string.IsNullOrEmpty(question.explain) ? "此题没有解释" : question.explain);

		yield return new WaitForSeconds(0.2f);
		oExplain.SetActive(true);
		oOptions.SetActive(false);
		
		yield return new WaitForSeconds(3f);
		questionList.Remove(question);
		yield return NextQuestion();
	}
	
	
}

}
