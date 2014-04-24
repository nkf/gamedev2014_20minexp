using UnityEngine;
using System.Collections;

public class HallMovement : MonoBehaviour {

    private const float IN_FRONT_OF_DOOR = -10.2f;
    private const float IN_FRONT_OF_CAR = -25.5f;
	private const float AFTER_STEP = -12.2f;
	private const float NEXT_TO_WIFE = -5.9f;

    public static bool DoorOpened;
	public static bool WifeSatisfied = false;

	void Update () {

		Debug.Log (transform.position);
	    var v = Input.GetAxis("Vertical");
	    var p = transform.position;

		if (p.z > IN_FRONT_OF_DOOR && p.z < 1.0f && WifeSatisfied) {
				p.z -= (v * Time.deltaTime);
				if (p.z > 1.0f)
						return;
				if (p.z < IN_FRONT_OF_DOOR && !DoorOpened)
						return;
		} else if (DoorOpened && p.z > IN_FRONT_OF_CAR) {
				p.z -= (v * Time.deltaTime * 2);
				if (p.z < IN_FRONT_OF_CAR)
						return;
		} else if (p.z > NEXT_TO_WIFE) {

			Debug.Log("does it compute");
			p.z -= (v * Time.deltaTime);
			if (p.z > 1.0f)
					return;
			if (p.z < NEXT_TO_WIFE && !WifeSatisfied)
					return;

		}
			if (p.z < AFTER_STEP) {
			p.y = 1.68f;
		} else p.y = 2.2f;



	    transform.position = p;


	}
}
