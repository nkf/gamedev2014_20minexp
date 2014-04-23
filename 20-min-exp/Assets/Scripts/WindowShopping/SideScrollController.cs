using System.Linq;
using UnityEngine;
using System.Collections;

public class SideScrollController : MonoBehaviour {

    private GameObject[] _wheels;
	void Start () {
		Toolbox touch = Toolbox.Instance; // TODO: Remove for final version
		Debug.Log ("Derp "+Toolbox.Instance.gameState.DayCounter);
		_wheels = GameObject.FindGameObjectsWithTag("Wheel");
		startTime = Time.time;
	}
    public float Acceleration = 2f;
    public float DeAcceleration = 1f;
    public float Braking = 5f;
    public float MinSpeed = 0;
    public float MaxSpeed = 100;
    public float WheelRotationFactor = 4;
    [HideInInspector]
    public float Speed = 0;

	private float startTime;
	private bool isLoading = false;

	// Update is called once per frame
	void Update () {
        var p = transform.position;
        var h = Input.GetAxis("Horizontal");
	    if (h > 0) {
	        Speed += h * Acceleration;
	    } else if (h < 0) {
	        Speed += h * Braking;
	    } else {
	        Speed -= DeAcceleration;
	    }
	    Speed = Mathf.Clamp(Speed, MinSpeed, MaxSpeed);
	    p.x += Speed * Time.deltaTime;
	    transform.position = p;
        RotateWheels(Speed*WheelRotationFactor);

		//TODO: Find another way and condition to get to the next level 
		if ((Time.time - startTime) > 5.0f && !isLoading) {
			// Increase because we want to load the afternoon table scene, which is actually on the same ingame day as the morning table scene before getting fired
			Toolbox.Instance.gameState.DayCounter++;
			Toolbox.Instance.levelController.Load(LevelController.TABLE);
			isLoading = true;
		}
	}

    private void RotateWheels(float degrees) {
        foreach (var wheel in _wheels) {
            var a = wheel.transform.localEulerAngles;
            a.z += degrees;
            wheel.transform.localEulerAngles = a;
        }
    }
}
