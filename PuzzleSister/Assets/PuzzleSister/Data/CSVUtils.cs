using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PuzzleSister {
  
  public class CSVUtils
  {
    public static string LINE_SPLIT = "\n";
    public static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    public static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    public static char[] TRIM_CHARS = { '\"', '\0' };
    public static string QUESTION_BEGIN = "#QuestionBegin";
    public static string QUESTION_LIST = "#QuestionList";
    public static string QUESTION_END = "#QuestionEnd";


    public static string ExtractPackage(string text) {
      var lines = Regex.Split(text, CSVUtils.LINE_SPLIT_RE);
      string[] pkgData = new string[] { lines[0], lines[2] };
      return String.Join(CSVUtils.LINE_SPLIT, pkgData);
    }

    public static string ExtractQuestions(string text) {
      var lines = Regex.Split(text, CSVUtils.LINE_SPLIT_RE);
      List<string> questionList = new List<string>();
      var begin = false;
      var listBegin = false;
      var end = false;
      foreach(var line in lines) {
        if (line.StartsWith(QUESTION_BEGIN)) {
          begin = true;
        } else if (line.StartsWith(QUESTION_LIST)) {
          listBegin = true;
        } else if (line.StartsWith(QUESTION_END)) {
          end = true;
        } else {
          if (end) {
            break;
          }
          if (listBegin) {
            questionList.Add(line);
          } else if(begin) {
            questionList.Add(line);
            begin = false;
          }
        }
      }
      return String.Join(CSVUtils.LINE_SPLIT, questionList.ToArray());
    }

    public static List<Dictionary<string, object>> Parse(string text)
    {
      var list = new List<Dictionary<string, object>>();

      var lines = Regex.Split(text, LINE_SPLIT_RE);

      if (lines.Length <= 1) return list;

      var header = Regex.Split(lines[0], SPLIT_RE);
      for (var i = 1; i < lines.Length; i++)
      {

        var values = Regex.Split(lines[i], SPLIT_RE);
        if (values.Length == 0 || values[0] == "") continue;

        var entry = new Dictionary<string, object>();
        for (var j = 0; j < header.Length && j < values.Length; j++)
        {
          string value = values[j];
          value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
          object finalvalue = value;
          int n;
          float f;
          if (int.TryParse(value, out n))
          {
            finalvalue = n;
          }
          else if (float.TryParse(value, out f))
          {
            finalvalue = f;
          }
          entry[header[j]] = finalvalue;
        }
        list.Add(entry);
      }
      return list;
    }
  }

}