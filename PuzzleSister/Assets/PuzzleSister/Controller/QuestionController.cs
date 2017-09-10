using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using System.IO;
using System;

namespace PuzzleSister {
	public class QuestionController : MonoBehaviour {

    private RoundService roundService;

    public TextEffect cPackageTitle;
    public TextEffect cEnergy;
    public TextEffect cProgress;
    public QuestionView questionView;
    public GameObject oDialogue;
    public TextEffect cDialogue;

    private Coroutine coroutineForStart;
    private bool dialgueConfirmed = false;
    private Question.Result answer = Question.Result.Unknow;

    void Start() {
      GlobalEvent.shared.AddListener((data) => {
        if (data.type == EventType.DialogueConfirmed) {
          dialgueConfirmed = true;
        } else {
          if (answer == Question.Result.Unknow) {
            switch(data.type) {
              case EventType.SelectOptionA: answer = Question.Result.A; break;
              case EventType.SelectOptionB: answer = Question.Result.B; break;
              case EventType.SelectOptionC: answer = Question.Result.C; break;
              case EventType.SelectOptionD: answer = Question.Result.D; break;
            }
          }
        }
      });
    }
    
    public void StartPackage(Package package) {
      questionView.gameObject.SetActive(false);
      cPackageTitle.SetText("「" + package.name + "」");

      roundService = new RoundService();
      roundService.Start(package);

      SetEnergy(roundService.Energy);
      SetProgress(roundService.Current, roundService.Total);
      ShowDialogue(false, false, "");

      coroutineForStart = StartCoroutine(StartQuestion());
    }

    public void StopAndReset() {
      if (coroutineForStart != null) {
        StopCoroutine(coroutineForStart);
      }
    }

    public IEnumerator StartQuestion() {
      yield return ShowDialogue(true, true, "答题马上要开始了，点击『对话框』任意位置开始答题");
      yield return WaitDialogueConfirm();
      
      while(roundService.HasNextQuestion()) {
        questionView.ShowQuestion(roundService.NextQuestion());
        questionView.gameObject.SetActive(true);
        SetProgress(roundService.Current, roundService.Total);
        yield return ShowDialogue(true, false, "请作答...");

        while(!roundService.IsCurrentCompleted) {
          yield return WaitForAnswer();
          roundService.SubmitAnswer(answer);
          
          if (!roundService.IsCorrect) {
            ShowDialogue(true, false, "好像不正确哦，请继续作答...");
          }

          // check answer and show other response
          SetEnergy(roundService.Energy);
        }

        // show explaination
        var resultText = "『" + (roundService.IsCorrect ? "回答正确" : "回答错误") + "』\n";
        yield return ShowDialogue(true, true, resultText + "『解释』" + roundService.CurrentQuestion.explain);
        yield return WaitDialogueConfirm();
      }
    }

    IEnumerator WaitDialogueConfirm() {
      yield return new WaitForSeconds(0.3f);
      dialgueConfirmed = false;
      while(!dialgueConfirmed) {
        yield return 1;
      }
    }

    IEnumerator WaitForAnswer() {
      yield return new WaitForSeconds(0.3f);
      answer = Question.Result.Unknow;
      questionView.SetInteractable(true);
      while(answer == Question.Result.Unknow) {
        yield return 1;
      }
      questionView.SetInteractable(true);
    }

    void SetEnergy(int energy) {
      char[] energyChars = "□□□□□".ToCharArray();
      for(int i=0; i<energy; i++) {
        energyChars[i] = '■';
      }
      cEnergy.SetText(new string(energyChars));
    }

    void SetProgress(int current, int total) {
      cProgress.SetText(current + "/" + total);
    }

    IEnumerator ShowDialogue(bool animate, bool pointer, string text) {
      cDialogue.SetText(text);
      oDialogue.transform.Find("Pointer").gameObject.SetActive(pointer);
      if (pointer) {
        while(cDialogue.IsShowing()) {
          yield return 1;
        }
      }
    }

  }

}