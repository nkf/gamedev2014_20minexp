using System.Linq;
using UnityEngine;
using System.Collections;

public class SideScrollController : MonoBehaviour {

	public static bool HasBoughtStuff = false;
	public static SideScrollController CONTROLLER;

    private GameObject[] _wheels;
	void Start () {
		Screen.lockCursor = false;
		Screen.showCursor = false;
	
		_wheels = GameObject.FindGameObjectsWithTag("Wheel");
		startTime = Time.time;
		SideScrollController.CONTROLLER = this;
	}
    public float Acceleration = 2f;
    public float DeAcceleration = 1f;
    public float Braking = 5f;
    public float MinSpeed = 0;
    public float MaxSpeed = 100;
    public float WheelRotationFactor = 4;
    [HideInInspector]
    public float Speed = 0;
	public float TimeAfterBuying = 30;

	private float startTime;
	public float StartTime { get {return startTime;} set {startTime = value;} }
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
		if ((Time.time - startTime) > TimeAfterBuying && !isLoading && SideScrollController.HasBoughtStuff) {
			// Increase because we want to load the afternoon table scene, which is actually on the same ingame day as the morning table scene before getting fired
			Toolbox.Instance.levelController.Load(LevelController.TABLE);
            Toolbox.Instance.gameState.DayCounter++;
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
