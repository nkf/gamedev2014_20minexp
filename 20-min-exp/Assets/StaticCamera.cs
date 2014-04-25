﻿using UnityEngine;
using System.Collections;

public class StaticCamera : MonoBehaviour {
	
	public static GameObject STATIC_CAMERA;
	
	public void Awake() {
		STATIC_CAMERA = this.gameObject;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void StartConversationStatic() {
		WifeAgent.JANE.GetComponent<WifeAgent>().StartConversation();
	}
	
}