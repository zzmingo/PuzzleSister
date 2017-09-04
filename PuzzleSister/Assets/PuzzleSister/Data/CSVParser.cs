using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

namespace PuzzleSister {

  public class CSVParser : Package.Parser {

    public List<Question> Parse(Package package, object data) {
      Regex questionRegex = new Regex(@"#QuestionBegin([\s\S]*?)#QuestionEnd", RegexOptions.IgnoreCase);
      var cvsStr = (data as TextAsset).text;
      cvsStr = questionRegex.Match(cvsStr).Groups[1].Value;
      var lines = Regex.Split(cvsStr, CSVUtils.LINE_SPLIT_RE);
      var lineList = new List<string>();
      lineList.AddRange(lines);
      lineList.RemoveAt(0);
      cvsStr = String.Join("\n\r", lineList.ToArray());
      Debug.Log(cvsStr);
      var cvsData = CSVUtils.Parse(cvsStr);
      var questionList = new List<Question>();
      var begin = false;
      foreach(var row in cvsData) {
        if ("#QuestionList".Equals(row["id"])) {
          begin = true;
        }
        else if (begin) {
          Question question = new Question();
          question.id = row["id"].ToString();
          question.title = row["title"].ToString();
          question.explain = row["explain"].ToString();
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
      }
      return questionList;
    }

  }

}