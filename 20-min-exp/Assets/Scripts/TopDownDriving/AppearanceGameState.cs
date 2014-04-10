using UnityEngine;
using System;
using System.Collections;

public class AppearanceGameState : MonoBehaviour {

	public static AppearanceGameState INSTANCE;

	protected float dayTimeStart;

	public float dayLengthInSecs; // Day length in seconds

	protected bool _inCutscene = false;
	public bool InCutscene { get { return _inCutscene; } set {_inCutscene = value; } }
	public float CurrentTime {
		get { return Time.time-dayTimeStart; }
	}



	protected string CurrentTimeToString {
		get {
			TimeSpan t = TimeSpan.FromSeconds( CurrentTime );
			return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", 
			                     t.Hours, 
			                     t.Minutes, 
			                     t.Seconds);
		}
	}

	// Use this for initialization
	void Start () {
		AppearanceGameState.INSTANCE = this;
		dayTimeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (CurrentTime > dayLengthInSecs)
			Debug.Log("YOU'RE LATE!!!!!!11!!");
	}

	void OnGUI() {
		int timerWidth = 200;
		Rect pos = new Rect((Screen.width/2)-(timerWidth/2), Screen.height/5, timerWidth, 20);
		GUI.Box(pos, "Time passed: "+CurrentTimeToString);
	}

	public void SpendTime(float time) {
		dayTimeStart -= time;  // Passed time is calculated based on (NOW - dayTimeStart), so push dayTimeStart back to increase passed time
	}
}
