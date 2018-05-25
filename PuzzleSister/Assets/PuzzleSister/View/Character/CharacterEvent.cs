using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
	public class CharacterEvent : MonoBehaviour {

		private Animator animator;

		void OnEnable() {
			if (!animator) {
				animator = GetComponent<Animator>();
				animator.keepAnimatorControllerStateOnDisable = true;
            }
			StartCoroutine(wait(Random.Range(3, 10)));
		}

        IEnumerator wait(float s) {
            yield return new WaitForSeconds(s);
            animator.SetBool("Blink", true);
        }

		void BlinkDisable() {
			animator.SetBool("Blink", false);
			StartCoroutine(wait(Random.Range(3, 10)));
		}
	}
}