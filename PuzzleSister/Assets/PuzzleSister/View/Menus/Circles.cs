using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace PuzzleSister{

    public class Circles : MonoBehaviour {

        [NotNullAttribute]public GameObject circleImg0;
        [NotNullAttribute]public GameObject circleImg1;

        void Start() {
            iTween.RotateBy(circleImg0, iTween.Hash(
                "z", 10.0f,
								"looptype", iTween.LoopType.loop,
                "speed", 5.0f
            ));
            iTween.RotateBy(circleImg1, iTween.Hash(
                "z", -30.0f,
								"looptype", iTween.LoopType.loop,
                "speed", 10.0f
            ));
        }
    }

}