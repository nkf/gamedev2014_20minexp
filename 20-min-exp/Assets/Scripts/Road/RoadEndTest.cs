using UnityEngine;
using System.Collections;

public class RoadEndTest : MonoBehaviour {

    
    public float RunTime = 30;

    void Start() {
        Invoke("GoNext", RunTime);
    }

    void GoNext() {
        Toolbox.Instance.levelController.Load(LevelController.PARKING);
    }
}
