using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

namespace PuzzleSister.QEditor {

  public class QEditorQuestionList : MonoBehaviour {

    public GameObject prefabQuestionRow;

    void Awake() {
      RefreshList();
      QEditorService.shared.OnQuestionChange.AddListener(RefreshList);
    }

    void RefreshList() {
      var questilList = QEditorService.shared.GetQuestions();
      var len = Math.Max(questilList.Count, transform.childCount);
      for(int i=0; i<len; i++) {
        if (i >= questilList.Count) {
          Destroy(transform.GetChild(i).gameObject);
        } else {
          Transform item;
          var question = questilList[i];
          if (i >= transform.childCount) {
            item = Instantiate(prefabQuestionRow, Vector3.zero, Quaternion.identity, transform).transform;
          } else {
            item = transform.GetChild(i);
          }
          AdaptItem(item, question);
        }
      }
    }

    void AdaptItem(Transform item, Question question) {
      item.name = question.id;
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