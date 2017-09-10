using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class QuestionView : MonoBehaviour {

    public TextEffect cTitle;
    public GameObject oOptions;

    public void ShowQuestion(Question question) {
      ShowTitle(question.title);
      ShowOptions(question);
      gameObject.ScaleFrom(new Vector3(0, 0, 0), 0.3f, 0);
    }

    public void SetInteractable(bool interactable) {
      foreach(Transform tOpt in oOptions.transform) {
        tOpt.GetComponent<Button>().interactable = interactable;
      }
    }
    
    void ShowTitle(string title) {
      cTitle.SetText(title);
    }

    void ShowOptions(Question question) {
      foreach(Transform tOpt in oOptions.transform) {
        tOpt.GetComponent<TextEffect>().SetText(tOpt.name + ". " + question.GetOptionByName(tOpt.name));
      }
    }

  }

}