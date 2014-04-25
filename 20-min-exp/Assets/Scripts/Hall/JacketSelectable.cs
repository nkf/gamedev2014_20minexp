using UnityEngine;
using System.Collections;

public class JacketSelectable : Selectable {

	public static JacketSelectable JACKET;
	
	public bool isTaken = false;
	
	void Start() {
		JacketSelectable.JACKET = this;
	}
	
	public override void Select() {
		JacketSelectable.JACKET.isTaken = true;
		Destroy(gameObject);
	}
}
