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
      Combo1,
      Combo2,
      Combo3,
      Combo4,
      Combo5,
      Combo6,
      Combo7,
      Combo8,
      Combo9,
      Combo10,
      UnfullNormal,
      OneNormal,
    }

    public Sprite[] unfullImages;
    public Sprite[] unfullMiddlles;

    private State currentState = State.Normal;
    private int energy = 5;

    void Start() {
      ResetState();
    }

    public void SetEnergy(int energy) {
      this.energy = energy;
      var gameObj = StateToGameObject(State.UnfullNormal);
      var img = gameObj.GetComponent<Image>();
      img.sprite = unfullImages[energy];

      gameObj = StateToGameObject(State.UnfullMiddle);
      img = gameObj.GetComponent<Image>();
      img.sprite = unfullMiddlles[energy];
    }

    public void ResetState() {
      currentState = State.Normal;
      foreach(Transform child in transform) {
        child.gameObject.SetActive(child.gameObject == StateToGameObject(State.Normal));
      }
    }

    public IEnumerator ShowState(State state) {
      if (currentState != state) {
        StateToGameObject(currentState).SetActive(false);
        // yield return InternalHideState(currentState);
        currentState = state;
        StateToGameObject(state).SetActive(true);
        yield return null;
        // yield return InternalShowState(state);
      }
    }

    public IEnumerator ResumeState(State state) {
      if (currentState != state) {
        if (true || currentState == State.One && state == State.OneNormal) {
          StateToGameObject(currentState).SetActive(false);
          currentState = state;
          StateToGameObject(state).SetActive(true);
        } else {
          yield return InternalHideState(currentState);
          currentState = state;
          yield return InternalShowState(state);
        }
      }
    }

    IEnumerator InternalShowState(State state) {
      var gameObj = StateToGameObject(state);
      gameObj.SetActive(true);
      var img = gameObj.GetComponent<Image>();
      img.canvasRenderer.SetAlpha(0.01f);
      img.CrossFadeAlpha(1f, 0.3f, false);
      yield return new WaitForSeconds(0.1f);
    }

    IEnumerator InternalHideState(State state) {
      var gameObj = StateToGameObject(state);
      gameObj.SetActive(true);
      yield return null;
      var img = gameObj.GetComponent<Image>();
      img.canvasRenderer.SetAlpha(1f);
      img.CrossFadeAlpha(0, 0.3f, false);
      yield return new WaitForSeconds(0.1f);
    }

    GameObject StateToGameObject(State state) {
      return transform.Find(Enum.GetName(typeof(State), state)).gameObject;
    }

  }

}