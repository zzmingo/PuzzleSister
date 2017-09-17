using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace PuzzleSister {

	public class QuestionView : MonoBehaviour {

    [NotNull] public TextEffect cTitle;
    [NotNull] public GameObject oOptions;
    [NotNull] public GameObject oMask;

    public void ShowQuestion(Question question) {
      ShowTitle(question.title);
      ShowOptions(question);
    }

    public void SetInteractable(bool interactable) {
      oMask.SetActive(!interactable);
    }

    public void HighlightOptions(Question.Result result) {
      foreach(Transform tOpt in oOptions.transform) {
        tOpt.GetComponent<Button>().enabled = false;
        // 5269FFFF
        if (result.Name().Equals(tOpt.name)) {
          var oFrame = tOpt.Find("Frame").gameObject;
          oFrame.SetActive(true);
          oFrame.ScaleFrom(new Vector3(1.3f, 1.3f, 1.3f), 0.3f, 0, EaseType.easeInQuart);
          StartCoroutine(FlashText(tOpt.Find("Text").GetComponent<Text>(), Const.COLOR_CORRECT));
        }
      }
    }

    public void DisableOption(Question.Result result) {
      var oOpt = oOptions.transform.Find(result.Name());
      oOpt.GetComponent<Button>().interactable = false;
      oOpt.Find("Text").GetComponent<Text>().color = Color.gray;
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
        tOpt.Find("Frame").gameObject.SetActive(false);
        var optionText = string.Format("{0}、{1}", tOpt.name, question.GetOptionByName(tOpt.name));
        tOpt.Find("Text").GetComponent<Text>().text = optionText;
        tOpt.Find("Text").GetComponent<Text>().color = Color.white;
      }
    }

  }

}