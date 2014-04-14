using UnityEngine;
using System.Collections;

public abstract class InvokableAction : MonoBehaviour {

	protected bool _actionAvailable = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.Equals(Car.PLAYER.gameObject)) {
			_actionAvailable = true;
		}
	}
	
	void OnTriggerExit(Collider collider) {
		if (collider.gameObject.Equals(Car.PLAYER.gameObject)) {
			_actionAvailable = false;
		}
	}
}
