using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace PuzzleSister {
	public class QuestionController : MonoBehaviour {

    private RoundService roundService;

    public TextEffect cPackageTitle;
    public TextEffect cEnergyBG;
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
      oDialogue.SetActive(false);

      roundService = new RoundService(package);
      roundService.Start();

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
      yield return ShowDialogue(true, true, "『答题回合』马上要开始了，点击『对话框』任意位置开始答题");
      yield return WaitDialogueConfirm();
      
      while(roundService.HasNextQuestion()) {
        questionView.gameObject.SetActive(true);
        questionView.ShowQuestion(roundService.NextQuestion());
        SetProgress(roundService.Current, roundService.Total);
        yield return ShowDialogue(false, false, "请作答...");

        while(!roundService.IsCurrentCompleted) {
          yield return WaitForAnswer();
          Debug.Log(answer);
          roundService.SubmitAnswer(answer);
          
          if (!roundService.IsCorrect) {
            questionView.DisableOption(answer);
            yield return ShowDialogue(false, false, "好像不正确哦，请继续作答...");
          }

          // check answer and show other response
          SetEnergy(roundService.Energy);
        }

        questionView.HighlightOptions(roundService.CurrentQuestion.result);

        // show explaination
        string resultTpl = "{1}，解释如下：\n『{2}』";
        string result = (roundService.IsCorrect ? "回答正确" : "回答错误");
        string dialogue = String.Format(
          resultTpl, Const.COLOR_CORRECT_HEX_STRING, result, roundService.CurrentQuestion.explain);
        yield return ShowDialogue(false, true, dialogue);
        yield return WaitDialogueConfirm();
      }

      // save completed
      var completedCount = 0;
      var pkgService = PackageService.For(roundService.package);
      pkgService.Load();
      foreach(var answerItem in roundService.CompletedList) {
        if (answerItem.completed) {
          completedCount ++;
          pkgService.SetCompleted(answerItem.question);
        }
      }
      pkgService.Save();

      // save progress
      var progressService = PackageProgressService.shared;
      progressService.Load();
      progressService.SetProgress(roundService.package.id, pkgService.CompletedCount, roundService.PackageQuestionCount);
      progressService.Save();

      // show ending dialogue
      string roundResult = "『答题回合』结束了，总共{0}题，本次完成{1}题，只有一次回答成功才算完成，点击『对话框』任意位置返回";
      roundResult = String.Format(roundResult, "" + roundService.Total, "" + completedCount);
      yield return ShowDialogue(true, true, roundResult);
      yield return WaitDialogueConfirm();
      GlobalEvent.shared.Invoke(EventType.QuestionPanelToPackageList);
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
      oDialogue.SetActive(true);
      cDialogue.SetText("");
      if (animate) {
        oDialogue.ScaleFrom(new Vector3(0, 1f, 1f), 0.2f, 0);
        yield return new WaitForSeconds(0.2f);
      }
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