using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace PuzzleSister {

  public class CharacterView : MonoBehaviour {

    public enum State {
      Zero,
      One,
      Two,
      Three,
      Four,
      UnfullMiddle,
      Normal,
      Combo2,
      Combo3,
      Combo4,
      Combo5,
      Combo6,
      Combo7,
      Combo8,
      Combo9,
    }

    private State currentState = State.Normal;

    void Start() {
      ResetState();
    }

    public void ResetState() {
      currentState = State.Normal;
      foreach(Transform child in transform) {
        child.gameObject.SetActive(child.gameObject == StateToGameObject(State.Normal));
      }
    }

    public IEnumerator ShowState(State state) {
      if (currentState != state) {
        yield return InternalHideState(currentState);
        currentState = state;
        yield return InternalShowState(state);
      }
    }

    public IEnumerator ResumeState(State state) {
      if (currentState != state) {
        yield return InternalHideState(currentState);
        currentState = state;
        yield return InternalShowState(state);
      }
    }

    IEnumerator InternalShowState(State state) {
      var gameObj = StateToGameObject(state);
      gameObj.SetActive(true);
      var img = gameObj.GetComponent<Image>();
      img.canvasRenderer.SetAlpha(0.01f);
      img.CrossFadeAlpha(1f, 0.5f, false);
      yield return new WaitForSeconds(0.3f);
    }

    IEnumerator InternalHideState(State state) {
      var gameObj = StateToGameObject(state);
      gameObj.SetActive(true);
      var img = gameObj.GetComponent<Image>();
      img.canvasRenderer.SetAlpha(1f);
      img.CrossFadeAlpha(0, 0.5f, false);
      yield return new WaitForSeconds(0.3f);
    }

    GameObject StateToGameObject(State state) {
      return transform.Find(Enum.GetName(typeof(State), state)).gameObject;
    }

  }

}