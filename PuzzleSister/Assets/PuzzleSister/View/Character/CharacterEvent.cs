using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PuzzleSister {
	public class CharacterEvent : MonoBehaviour {

		private Animator animator;

		void OnEnable() {
			if (!animator) {
				animator = GetComponent<Animator>();
            }
			StartCoroutine(wait(Random.Range(3, 10)));
		}

		void EyesNormal() {
			StartCoroutine(wait(Random.Range(3, 10)));
		}

        IEnumerator wait(float s) {
            animator.SetBool("Blink", false);
            yield return new WaitForSeconds(s);
            animator.SetBool("Blink", true);
        }

        void OnDisable() {
            animator.Rebind();
            animator.ResetTrigger("Blink");
        }
	}
}