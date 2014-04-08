using UnityEngine;
using System.Collections;

public class HallMovement : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
	    var v = Input.GetAxis("Vertical");
	    var p = transform.position;
	    p.z = p.z - v * Time.deltaTime;
	    transform.position = p;

	}
}
