using UnityEngine;
using System.Collections;

public class CarSelectable : Selectable {

	public override void Select() {
	    if (Toolbox.Instance.gameState.DayCounter == GameState.REGULAR_DAY) {
	        Toolbox.Instance.levelController.Load(LevelController.ROAD);
	    } else {
	        Toolbox.Instance.levelController.Load(LevelController.PARKING);
	    }
		audio.Play();
	}
}
