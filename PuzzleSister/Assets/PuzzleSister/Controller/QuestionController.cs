using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

namespace PuzzleSister {
	public class QuestionController : MonoBehaviour {

    private RoundService roundService;

    [NotNull] public TextEffect cPackageTitle;
    [NotNull] public TextEffect cEnergy;
    [NotNull] public QuestionView questionView;
    [NotNull] public GameObject oDialogue;
    [NotNull] public TextEffect cDialogue;
    [NotNull] public GameObject oDialogueMask;
    [NotNull] public CharacterController characterController;
    // [NotNull] public AudioClip correctClip;
    // [NotNull] public AudioClip wrongClip;

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
            if (answer != Question.Result.Unknow) {
              GlobalEvent.shared.Invoke(EventType.PlayLiteHitAudio);
            }
          }
        }
      });
    }
    
    public void StartPackage(Package package) {

      questionView.gameObject.SetActive(false);
      // cPackageTitle.SetText("「" + package.name + "」");
      oDialogue.SetActive(false);
      oDialogueMask.SetActive(false);

      characterController.ResetState();

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
      GlobalEvent.shared.Invoke(EventType.PlayStartAudio);
      yield return ShowDialogue(true, true, "『答题回合』马上要开始了，点击『对话框』任意位置开始答题");
      yield return WaitDialogueConfirm();
      
      while(roundService.HasNextQuestion() && !roundService.IsEnergyEmpty()) {
        questionView.gameObject.SetActive(true);
        questionView.ShowQuestion(roundService.NextQuestion());
        SetProgress(roundService.Current, roundService.Total);
        yield return ShowDialogue(false, false, "请作答...");

        while(!roundService.IsCurrentCompleted) {
          Debug.Log(roundService.Current + "," + roundService.CurrentQuestion.result);
          yield return WaitForAnswer();
          roundService.SubmitAnswer(answer);

          // check answer and show other response
          SetEnergy(roundService.Energy);

          if (roundService.IsEnergyEmpty()) {
            yield return characterController.ShowStateFor(roundService);
            break;
          }
          
          if (!roundService.IsCorrect) {
            // Utils.PlayClip(wrongClip);
            questionView.DisableOption(answer);
            yield return characterController.ShowStateFor(roundService);
            yield return ShowDialogue(false, true, "好像不正确哦");
            yield return WaitDialogueConfirm();
            StartCoroutine(characterController.ResumeStateFor(roundService));
            yield return ShowDialogue(false, false, "请继续作答...");
          } else {
            // Utils.PlayClip(correctClip);
            yield return characterController.ShowStateFor(roundService);
          }
        }

        questionView.HighlightOptions(roundService.CurrentQuestion.result);

        // show explaination
        string resultTpl = "{1}，解释如下：\n『{2}』";
        string result = (roundService.IsCorrect ? "回答正确" : "回答错误");
        string dialogue = String.Format(
          resultTpl, Const.COLOR_CORRECT_HEX_STRING, result, roundService.CurrentQuestion.explain);
        yield return ShowDialogue(false, true, dialogue);
        yield return WaitDialogueConfirm();

        if (roundService.IsCorrect) {
          yield return characterController.ResumeStateFor(roundService);
        }
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
      oDialogueMask.SetActive(true);
      while(!dialgueConfirmed) {
        yield return 1;
      }
      oDialogueMask.SetActive(false);
    }

    IEnumerator WaitForAnswer() {
      yield return new WaitForSeconds(0.3f);
      answer = Question.Result.Unknow;
      questionView.SetInteractable(true);
      while(answer == Question.Result.Unknow) {
        yield return 1;
      }
      questionView.SetInteractable(false);
    }

    void SetEnergy(int energy) {
      char[] energyChars = "□□□□□".ToCharArray();
      for(int i=0; i<energy; i++) {
        energyChars[i] = '■';
      }
      cEnergy.SetText(new string(energyChars));
    }

    void SetProgress(int current, int total) {
      var title = string.Format("{0}：{1}/{2}", roundService.package.name, current, total);
      cPackageTitle.SetText(title);
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