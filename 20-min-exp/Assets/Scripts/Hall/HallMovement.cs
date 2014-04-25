using UnityEngine;
using System.Collections;

public class HallMovement : MonoBehaviour {

    private const float IN_FRONT_OF_DOOR = -10.2f;
    private const float IN_FRONT_OF_CAR = -25.5f;
	private const float AFTER_STEP = -12.2f;
	private const float NEXT_TO_WIFE = -5.9f;

	public static bool WifeSatisfied = false;

	void Update () {

	    var v = Input.GetAxis("Vertical");
	    var p = transform.position;

		// Disable movement if talking to waifu
		if (WifeHallAgent.JANE.IsRunning)
			return;

		// Go between door and start position
		if (p.z > IN_FRONT_OF_DOOR && p.z < 1.0f && WifeSatisfied) {
				p.z -= (v * Time.deltaTime);
				if (p.z > 1.0f)
					return;
				if (p.z < IN_FRONT_OF_DOOR &&
			    	!DoorSelectable.FRONT_DOOR.isOpened)
					return;
		} else if (DoorSelectable.FRONT_DOOR.isOpened && p.z > IN_FRONT_OF_CAR) {
			// Go outside
				p.z -= (v * Time.deltaTime * 2);
				if (p.z < IN_FRONT_OF_CAR)
					return;
		} else if (p.z > NEXT_TO_WIFE) {
			// Be stopped by wife
			p.z -= (v * Time.deltaTime);
			if (p.z > 1.0f)
					return;
			if (p.z < NEXT_TO_WIFE && !WifeSatisfied)
			{
				if (!WifeHallAgent.JANE.IsRunning) {
					WifeHallAgent.JANE.StartConversation();
				}
				return;
			}

		}

		if (p.z < AFTER_STEP) {
			p.y = 1.68f;
		} else p.y = 2.2f;
			transform.position = p;
		}
}
