using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;

namespace PuzzleSister {
	public class GameController : MonoBehaviour {
	
	public GameObject oQuestionPanel;
	public GameObject oQuestionCT;
	public GameObject oStartScreen;

	private int totalScore = 0;
	private int totalCount = 0;
	private Coroutine questionCoroutine;
	private Coroutine scoreAnimCoroutine;
	private Question.Result result = Question.Result.Unknow;
	private List<Question> questionList;

	void Start() {
		oStartScreen.SetActive(true);
		oQuestionPanel.SetActive(false);
	}

	public void OnTapBtnStart() {
		oStartScreen.SetActive(false);
		oQuestionPanel.SetActive(true);

		StartQuestionGame();
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

	void StartQuestionGame() {
		Repository.shared.LoadPackages();
		Package pkg = Repository.shared.GetPackageById("PKG0001");
		questionList = pkg.Load();
		totalCount = questionList.Count;
		questionCoroutine = StartCoroutine(NextQuestion());
	}

	IEnumerator NextQuestion() {
		if (questionList.Count > 0) {
			yield return HandleNextQuestion();
		}
	}

	IEnumerator HandleNextQuestion() {
		result = Question.Result.Unknow;
		int idx = Random.Range(0, questionList.Count);
		var question = questionList[idx];
		questionList.Remove(question);

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

		while(result == Question.Result.Unknow) {
			yield return 1;
		}

		// explaination
		oExplain.SetActive(false);
		var cExplainText = oExplain.GetComponent<Text>();
		var resultText = result == question.result ? "回答正确" : "回答错误";
		if (result != question.result) {
			cExplainText.text = resultText + "\r\n\r\n" + (string.IsNullOrEmpty(question.explain) ? "不解释" : question.explain);
		} else {
			cExplainText.text = resultText;
		}

		if (result == question.result) {
			// TODO
		}
		

		yield return new WaitForSeconds(0.2f);
		oExplain.SetActive(true);
		oOptions.SetActive(false);
		if (result == question.result) {
			yield return new WaitForSeconds(1f);
		} else {
			yield return new WaitForSeconds(3f);
		}

		// TODO: dispose question

		yield return NextQuestion();
	}
	
	
}

}
