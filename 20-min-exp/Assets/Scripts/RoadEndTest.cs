using UnityEngine;
using System.Collections;

public class RoadEndTest : MonoBehaviour {

    
    public float RunTime = 30;
    private float _start;
    private float _end;
    private bool _inTransistion = false;
	void Start () {
	    _start = Time.time;
	    _end = _start + RunTime;
	    LevelLoader.Load("Parking");
	}
	
	// Update is called once per frame
	void Update () {
	    if (Time.time > _end && !_inTransistion) {
	        if (LevelLoader.Status == LoadStatus.Done) {
	            _inTransistion = true;
	            StartCoroutine(Camera.main.FadeToBlack(3, LevelLoader.Switch));
	        } else {
	            _end++;
	        }
	    }
	}
}
