using UnityEngine;
using System.Collections;

public class KeysSelectable : Selectable {

	public static KeysSelectable KEYS;
	
	public bool isTaken = false;
	
	void Start() {
		KeysSelectable.KEYS = this;
	}
	
	public override void Select() {
		KeysSelectable.KEYS.isTaken = true;
		Destroy(gameObject);
	}
}
