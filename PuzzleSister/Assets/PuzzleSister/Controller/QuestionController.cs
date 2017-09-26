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
    [NotNull] public QuestionView questionView;
    [NotNull] public GameObject oDialogue;
    [NotNull] public TextEffect cDialogue;
    [NotNull] public GameObject oDialogueMask;
    [NotNull] public CharacterController characterController;
    [NotNull] public AudioClip correctClip;
    [NotNull] public AudioClip wrongClip;

    private Coroutine coroutineForStart;
    private bool dialgueConfirmed = false;
    private Question.Result answer = Question.Result.Unknow;
    private VoiceSuite voiceSuite;

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
      voiceSuite = VoiceSuite.LoadBySetting();

      questionView.gameObject.SetActive(false);
      // cPackageTitle.SetText("「" + package.name + "」");
      oDialogue.SetActive(false);
      oDialogueMask.SetActive(false);

      characterController.ResetState();

      roundService = new RoundService(package);
      roundService.Start();

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

          if (!roundService.IsCorrect) {
            Utils.PlayClip(wrongClip, 0.4f);
          } else {
            Utils.PlayClip(correctClip, 0.4f);
          }

          if (roundService.IsEnergyEmpty()) {
            questionView.DisableOption(answer);
            yield return characterController.ShowStateFor(roundService);
            VoicePlayer.shared.Play(voiceSuite.X0Clips.RandomOne());
            break;
          }
          
          if (!roundService.IsCorrect) {
            questionView.DisableOption(answer);
            yield return characterController.ShowStateFor(roundService);
            switch(roundService.Energy) {
              case 1: VoicePlayer.shared.Play(voiceSuite.X1Clips.RandomOne()); break;
              case 2: VoicePlayer.shared.Play(voiceSuite.X2Clips.RandomOne()); break;
              case 3: VoicePlayer.shared.Play(voiceSuite.X3Clips.RandomOne()); break;
              case 4: VoicePlayer.shared.Play(voiceSuite.X4Clips.RandomOne()); break;
            }
            yield return ShowDialogue(false, true, "好像不正确哦");
            yield return WaitDialogueConfirm();
            StartCoroutine(characterController.ResumeStateFor(roundService));
            yield return ShowDialogue(false, false, "请继续作答...");
          } else {
            yield return characterController.ShowStateFor(roundService);
            if (roundService.Energy < 5) {
              VoicePlayer.shared.Play(voiceSuite.XYClips.RandomOne());
            } else {
              if (roundService.Current == 1) {
                VoicePlayer.shared.Play(voiceSuite.Y1Clips.RandomOne());
              } else {
                if (roundService.Combo <= 1) {
                  VoicePlayer.shared.Play(voiceSuite.X5Clips.RandomOne());
                } else {
                  switch(roundService.Combo) {
                    case 2: VoicePlayer.shared.Play(voiceSuite.Y2Clips.RandomOne()); break;
                    case 3: VoicePlayer.shared.Play(voiceSuite.Y3Clips.RandomOne()); break;
                    case 4: VoicePlayer.shared.Play(voiceSuite.Y4Clips.RandomOne()); break;
                    case 5: VoicePlayer.shared.Play(voiceSuite.Y5Clips.RandomOne()); break;
                    case 6: VoicePlayer.shared.Play(voiceSuite.Y6Clips.RandomOne()); break;
                    case 7: VoicePlayer.shared.Play(voiceSuite.Y7Clips.RandomOne()); break;
                    case 8: VoicePlayer.shared.Play(voiceSuite.Y8Clips.RandomOne()); break;
                    case 9: VoicePlayer.shared.Play(voiceSuite.Y9Clips.RandomOne()); break;
                    case 10: VoicePlayer.shared.Play(voiceSuite.Y10Clips.RandomOne()); break;
                  }
                }
              }
            }
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
      if (!roundService.package.temporary) {
        var progressService = PackageProgressService.shared;
        progressService.Load();
        progressService.SetProgress(roundService.package.id, pkgService.CompletedCount, roundService.PackageQuestionCount);
        progressService.Save();
      }

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