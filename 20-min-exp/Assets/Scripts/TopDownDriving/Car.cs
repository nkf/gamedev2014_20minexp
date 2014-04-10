using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour {

	public static Car PLAYER;

	// Use this for initialization
	void Start () {
		PLAYER = this;
	}
}
