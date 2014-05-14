using UnityEngine;
using System.Collections;

public class DoorSelectable : Selectable {

	public static DoorSelectable FRONT_DOOR;

	public bool isOpened = false;

	void Start() {
		DoorSelectable.FRONT_DOOR = this;
	}

	public override void Select() {
		if (KeysSelectable.KEYS.isTaken && JacketSelectable.JACKET.isTaken && !isOpened) {
			FRONT_DOOR.isOpened = true;
            audio.Play();
		    animation.Play();
		}
		else if (!KeysSelectable.KEYS.isTaken && !JacketSelectable.JACKET.isTaken) {
			StartCoroutine(Camera.main.ShowCenterText("I am not going anywhere without my coat and keys...", () => {}));
		}
		else if (!KeysSelectable.KEYS.isTaken) {
			StartCoroutine(Camera.main.ShowCenterText("Where are those damn keys?", () => {}));
		}
		else if (!JacketSelectable.JACKET.isTaken) {
			StartCoroutine(Camera.main.ShowCenterText("It is cold outside, I should get my coat.", () => {}));
		}
	}
}
