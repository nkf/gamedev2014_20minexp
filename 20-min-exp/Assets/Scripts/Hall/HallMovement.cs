using UnityEngine;
using System.Collections;

public class HallMovement : MonoBehaviour {

    private const float IN_FRONT_OF_DOOR = -6.7f;
    private const float IN_FRONT_OF_CAR = -25.5f;
    public static bool DoorOpened;
	void Update () {
	    var v = Input.GetAxis("Vertical");
	    var p = transform.position;
	    if (p.z > IN_FRONT_OF_DOOR && p.z < 1.0f) {
	        p.z -= (v*Time.deltaTime);
	        if (p.z > 1.0f) return;
	        if (p.z < IN_FRONT_OF_DOOR && !DoorOpened) return;
	    } else if (DoorOpened && p.z > IN_FRONT_OF_CAR) {
	        p.z -= (v*Time.deltaTime*2);
	        if (p.z < IN_FRONT_OF_CAR) return;
	    }
	    transform.position = p;

	}
}
