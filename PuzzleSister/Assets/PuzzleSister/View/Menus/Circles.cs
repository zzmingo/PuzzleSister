using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister{

    public class Circles : MonoBehaviour {

        [NotNullAttribute]public GameObject circleImg0;
        [NotNullAttribute]public GameObject circleImg1;

        void Start() {
            iTween.RotateBy(circleImg0, iTween.Hash(
                "z", 10,
                "looptype", iTween.LoopType.pingPong,
                "time", 60.0f,
                "easetype", iTween.EaseType.easeInOutSine
            ));
            iTween.RotateBy(circleImg1, iTween.Hash(
                "z", -30,
                "looptype", iTween.LoopType.pingPong,
                "time", 80.0f,
                "easetype", iTween.EaseType.easeInOutSine
            ));
        }
    }

}