using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class QuestionView : MonoBehaviour {

    [NotNull] public TextEffect cTitle;
    [NotNull] public GameObject oOptions;

    public void ShowQuestion(Question question) {
      ShowTitle(question.title);
      ShowOptions(question);
      gameObject.ScaleFrom(new Vector3(0, 0, 0), 0.3f, 0);
    }

    public void SetInteractable(bool interactable) {
      foreach(Transform tOpt in oOptions.transform) {
        tOpt.GetComponent<Button>().enabled = interactable;
      }
    }

    public void HighlightOptions(Question.Result result) {
      foreach(Transform tOpt in oOptions.transform) {
        tOpt.GetComponent<Button>().enabled = false;
        // 5269FFFF
        if (result.Name().Equals(tOpt.name)) {
          StartCoroutine(FlashText(tOpt.GetComponent<Text>(), Const.COLOR_CORRECT));
        } else {
          tOpt.GetComponent<Text>().color = Color.white;
        }
      }
    }

    public void DisableOption(Question.Result result) {
      oOptions.transform.Find(result.Name()).GetComponent<Button>().interactable = false;
    }

    IEnumerator FlashText(Text text, Color color, int times = 8) {
      if (times == 0) {
        text.color = color;
      } else {
        if (text.color.Equals(color)) {
          text.color = Color.white;
        } else {
          text.color = color;
        }
        yield return new WaitForSeconds(0.1f);
        yield return FlashText(text, color, times-1);
      }
    }
    
    void ShowTitle(string title) {
      cTitle.SetText(title);
    }

    void ShowOptions(Question question) {
      foreach(Transform tOpt in oOptions.transform) {
        tOpt.GetComponent<Button>().enabled = true;
        tOpt.GetComponent<Button>().interactable = true;
        tOpt.GetComponent<Text>().color = Color.white;
        tOpt.GetComponent<TextEffect>().SetText(tOpt.name + ". " + question.GetOptionByName(tOpt.name));
      }
    }

  }

}