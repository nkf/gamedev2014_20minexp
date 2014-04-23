using UnityEngine;
using System.Collections;

public class CarSelectable : Selectable {

	public override void Select() {
		Toolbox.Instance.levelController.Load(LevelController.ROAD);
	}
}
