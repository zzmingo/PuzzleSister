using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PuzzleSister {

  /// <summary>
  /// 回合答题服务
  /// </summary>
  public class RoundService {
    
    public int Heart { get { return heart; } }
    public int Combo { get { return combo; } }
    public bool IsCorrect { get { return correct; } }
    public bool IsCurrentCompleted { get { return correct && answerTimes == 3; } }
    
    private List<Question> questionList;
    private int heart = 5;
    private int combo = 0;
    private int totalCount = 10;
    private int currentNum = 0;
    private Question currentQuestion;
    private int answerTimes = 0;
    private bool correct = false;
    private List<AnswerItem> completedList;

    public void Start(Package package) {
      questionList = package.Load();
      completedList = new List<AnswerItem>();
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
        if (heart < 5) heart++;
      } else {
        if (heart > 0) heart --;
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