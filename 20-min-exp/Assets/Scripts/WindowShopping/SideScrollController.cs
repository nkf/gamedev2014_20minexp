using System.Linq;
using UnityEngine;
using System.Collections;

public class SideScrollController : MonoBehaviour {

    private GameObject[] _wheels;
	void Start () {
	    _wheels = GameObject.FindGameObjectsWithTag("Wheel");
	}
    public float Acceleration = 2f;
    public float DeAcceleration = 1f;
    public float Braking = 5f;
    public float MinSpeed = 0;
    public float MaxSpeed = 100;
    public float WheelRotationFactor = 4;
    [HideInInspector]
    public float Speed = 0;
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
	}

    private void RotateWheels(float degrees) {
        foreach (var wheel in _wheels) {
            var a = wheel.transform.localEulerAngles;
            a.z += degrees;
            wheel.transform.localEulerAngles = a;
        }
    }
}
