using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

namespace PuzzleSister {

  public class CSVParser : Package.Parser {

    public List<Question> Parse(Package package, object data) {
      var csvStr = data.ToString();
      var questionDictList = CSVUtils.Parse(csvStr);
      var questionList = new List<Question>();
      foreach(var row in questionDictList) {
        foreach(var entry in row) {
          Debug.Log(entry.Key + " => " + entry.Value);
        }
        Question question = new Question();
        question.id = row["id"].ToString();
        question.title = row["title"].ToString();
        question.explain = row["explain"].ToString();
        question.result = (Question.Result) Enum.Parse(typeof(Question.Result), row["result"].ToString(), true);
        switch(row["result"].ToString()) {
          case "A": question.result = Question.Result.A; break;
          case "B": question.result = Question.Result.B; break;
          case "C": question.result = Question.Result.C; break;
          case "D": question.result = Question.Result.D; break;
        }
        question.optionA = row["A"].ToString();
        question.optionB = row["B"].ToString();
        question.optionC = row["C"].ToString();
        question.optionD = row["D"].ToString();
        questionList.Add(question);
      }
      return questionList;
    }

  }

}