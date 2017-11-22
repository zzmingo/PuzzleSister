using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
  
  public class Question {

    public enum Result {
      Unknow, A, B, C, D
    }

    public string id;
    public string title;
    public Result result;
    public string explain;
    public string optionA;
    public string optionB;
    public string optionC;
    public string optionD;

    public string[] options {
      get {
        return new string[] { optionA, optionB, optionC, optionD };
      }
    }
    
    [HideInInspector] public bool completed;
    
    public string GetOptionByName(string name) {
      switch(name) {
        case "A": return optionA;
        case "B": return optionB;
        case "C": return optionC;
        case "D": return optionD;
        default: throw new UnityException("Unknow option name: " + name);
      }
    }

    public Question Clone() {
      Question question = new Question();
      question.id = id;
      question.title = title;
      question.result = result;
      question.explain = explain;
      question.optionA = optionA;
      question.optionB = optionB;
      question.optionC = optionC;
      question.optionD = optionD;
      return question;
    }
  }

  public static class ResultExtensions {

    public static string Name(this Question.Result result) {
      switch(result) {
        case Question.Result.A:
          return "A";
        case Question.Result.B:
          return "B";
        case Question.Result.C:
          return "C";
        case Question.Result.D:
          return "D";
        default:
          return "Unknow";
      }
    } 
  }

}
