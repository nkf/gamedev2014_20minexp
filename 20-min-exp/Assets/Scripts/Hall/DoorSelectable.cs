using UnityEngine;
using System.Collections;

public class DoorSelectable : Selectable {

	public override void Select() {
	    HallMovement.DoorOpened = true;
        Destroy(gameObject);
	}
}
