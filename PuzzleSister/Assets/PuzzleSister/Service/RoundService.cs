using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {

  /// <summary>
  /// 回合答题服务
  /// </summary>
  public class RoundService {

    public int PackageQuestionCount { get { return packageQuestionCount; } }
    public int Energy { get { return energy; } }
    public int Total { get { return totalCount; } }
    public int Current { get { return currentNum; } }
    public Question CurrentQuestion { get { return currentQuestion; } }
    public int Combo { get { return combo; } }
    public bool IsCorrect { get { return correct; } }
    public bool IsCurrentCompleted { get { return correct || answerTimes == 3; } }
    public List<AnswerItem> CompletedList { get { return completedList; } }
    public bool IsEnergyEmpty() { return Energy == 0; }
    
    public readonly Package package;
    private int packageQuestionCount;
    private List<Question> questionList;
    private int energy = 5;
    private int combo = 0;
    private int totalCount = 10;
    private int currentNum = 0;
    private Question currentQuestion;
    private int answerTimes = 0;
    private bool correct = false;
    private List<AnswerItem> completedList;

    public RoundService(Package package) {
      this.package = package;
    }

    public void Start() {
      questionList = package.Load();
      packageQuestionCount = questionList.Count;
      List<Question> filteredList = new List<Question>();
      var pkgService = PackageService.For(package);
      pkgService.Load();
      foreach(var question in questionList) {
        if (!pkgService.IsComplete(question)) {
          filteredList.Add(question);
        }
      }
      questionList = filteredList;
      completedList = new List<AnswerItem>();
      totalCount = questionList.Count >= totalCount ? totalCount : questionList.Count;
    }

    public bool HasNextQuestion() {
      return currentNum < totalCount;
    }

    public Question NextQuestion() {
      currentNum ++;
      answerTimes = 0;
      correct = false;
		  currentQuestion = questionList[Random.Range(0, questionList.Count)];
      questionList.Remove(currentQuestion);
      return currentQuestion;
    }

    public void SubmitAnswer(Question.Result answer) {
      correct = answer == currentQuestion.result;
      if (correct) {
        if (energy < 5) energy++;
      } else {
        if (energy > 0) energy --;
      }
      answerTimes ++;
      if (answerTimes == 1 && correct) {
        combo ++;
      }
      if (!correct) {
        combo = 0;
      }
      if (IsCurrentCompleted) {
        var answerItem = new AnswerItem();
        answerItem.question = currentQuestion;
        answerItem.completed = correct && answerTimes == 1;
        completedList.Add(answerItem);
      }
    }

    public class AnswerItem {
      public Question question;
      public bool completed;
    }

  }

}