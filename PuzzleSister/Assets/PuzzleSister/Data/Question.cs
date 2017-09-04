using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
  
  public class Question {

    public enum Result {
      Unknow, A, B, C, D
    }

    public string package;
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
    
  }

}
