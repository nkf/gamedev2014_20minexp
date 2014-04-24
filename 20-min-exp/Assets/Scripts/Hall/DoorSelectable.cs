using UnityEngine;
using System.Collections;

public class DoorSelectable : Selectable {

	public override void Select() {
		Debug.Log ("penis?");
	    HallMovement.DoorOpened = true;
        Destroy(gameObject);
	}
}
