using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using PuzzleSister;
#if UNITY_STANDALONE
using Steamworks;
#endif

namespace PuzzleSister.UGCEditor {

  public class QuestionForm : MonoBehaviour {

    public Question question = new Question();

    private bool blockingFormData = false;

    public void ResetDataAndUpdateUI() {
      ResetFormData();
      UpdateFormUI();
    }

    public void ResetFormData() {
      question = new Question();
      question.result = Question.Result.A;
    }

    public void UpdateFormUI() {
      blockingFormData = true;
      this.Query<InputField>("Content/InputField-Title").text = question.title;
      this.Query<InputField>("Content/InputField-A").text = question.optionA;
      this.Query<InputField>("Content/InputField-B").text = question.optionB;
      this.Query<InputField>("Content/InputField-C").text = question.optionC;
      this.Query<InputField>("Content/InputField-D").text = question.optionD;
      this.Query<InputField>("Content/InputField-Explain").text = question.explain;
      this.Query<Toggle>("Content/Result/" + question.result).isOn = true;
      blockingFormData = false;
    }

    public void UpdateFormData() {
      if (blockingFormData) {
        return;
      }
      question.title = this.Query<InputField>("Content/InputField-Title").text;
      question.optionA = this.Query<InputField>("Content/InputField-A").text;
      question.optionB = this.Query<InputField>("Content/InputField-B").text;
      question.optionC = this.Query<InputField>("Content/InputField-C").text;
      question.optionD = this.Query<InputField>("Content/InputField-D").text;
      question.explain = this.Query<InputField>("Content/InputField-Explain").text;
      var toggles = this.Query<ToggleGroup>("Content/Result").ActiveToggles();
      foreach(var toggle in toggles) {
        if (toggle.isOn) {
          question.result = (Question.Result) Enum.Parse(typeof(Question.Result), toggle.name);
          break;
        }
      }
    }

  }

}