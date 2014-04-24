using UnityEngine;
using System.Collections;

public class WifeSelectable : Selectable {
	
	public override void Select() {

		HallMovement.WifeSatisfied = true;
		Destroy(gameObject);
	}
}
