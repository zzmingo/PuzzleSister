using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.UGCEditor {

  public class QuestionList : MonoBehaviour {

    public GameObject prefabQuestionRow;

    public void InitList(List<Question> questionList) {
      var len = Math.Max(questionList.Count, transform.childCount);
      for(int i=0; i<len; i++) {
        if (i >= questionList.Count) {
          Destroy(transform.GetChild(i).gameObject);
        } else {
          Transform item;
          var question = questionList[i];
          if (i >= transform.childCount) {
            item = Instantiate(prefabQuestionRow, Vector3.zero, Quaternion.identity, transform).transform;
          } else {
            item = transform.GetChild(i);
          }
          AdaptItem(i + 1, item, question);
        }
      }
    }

    void AdaptItem(int index, Transform item, Question question) {
      item.name = question.id;
			item.GetComponent<ItemView>().itemData = question;
			item.Query<Text>("Index").text = "" + index;
      item.Query<Text>("Title").text = question.title;
      item.Query<Text>("A").text = question.optionA;
      item.Query<Text>("B").text = question.optionB;
      item.Query<Text>("C").text = question.optionC;
      item.Query<Text>("D").text = question.optionD;
      item.Query<Text>("Explain").text = question.explain;
      item.Query<Text>("Result").text = question.result.Name();
    }

  }

}