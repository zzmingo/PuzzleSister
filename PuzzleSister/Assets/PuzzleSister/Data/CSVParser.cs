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
        Question question = new Question();
        question.id = row["id"].ToString();
        question.title = row["title"].ToString();
        question.explain = row["explain"].ToString();
        question.result = (Question.Result) Enum.Parse(typeof(Question.Result), row["result"].ToString(), true);
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