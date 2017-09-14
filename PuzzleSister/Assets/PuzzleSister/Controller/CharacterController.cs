using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace PuzzleSister {

	public class CharacterController : MonoBehaviour {

    [NotNull] public CharacterView characterView;

    public void ResetState() {
      characterView.ResetState();
    }

    public IEnumerator ShowStateFor(RoundService roundService) {
      CharacterView.State state = CharacterView.State.Normal;
      int energy = roundService.Energy;
      int combo = roundService.Combo;
      bool correct = roundService.IsCorrect;

      if (correct) {
        if (energy <= 5 && combo <= 1) {
          state = CharacterView.State.UnfullMiddle;
        } else {
          switch(combo) {
            case 2: state = CharacterView.State.Combo2; break;
            case 3: state = CharacterView.State.Combo3; break;
            case 4: state = CharacterView.State.Combo4; break;
            case 5: state = CharacterView.State.Combo5; break;
            case 6: state = CharacterView.State.Combo6; break;
            case 7: state = CharacterView.State.Combo7; break;
            case 8: state = CharacterView.State.Combo8; break;
            case 9: state = CharacterView.State.Combo9; break;
          }
        }
      } else {
        switch(energy) {
          case 0: state = CharacterView.State.Zero; break;
          case 1: state = CharacterView.State.One; break;
          case 2: state = CharacterView.State.Two; break;
          case 3: state = CharacterView.State.Three; break;
          case 4: state = CharacterView.State.Four; break;
        }
      }
      yield return characterView.ShowState(state);
    }

    public IEnumerator ResumeStateFor(RoundService roundService) {
      if (roundService.Energy >= 5) {
        yield return characterView.ResumeState(CharacterView.State.Normal);
      } else {
        yield return characterView.ResumeState(CharacterView.State.Four);
      }
    }

  }

}