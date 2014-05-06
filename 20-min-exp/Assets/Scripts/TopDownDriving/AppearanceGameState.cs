using UnityEngine;
using System;
using System.Collections;

public class AppearanceGameState : MonoBehaviour {

	public static AppearanceGameState INSTANCE;

	protected float dayTimeStart;

	public float dayLengthInSecs; // Day length in seconds

	protected bool _inCutscene = false;
	public bool InCutscene { get { return _inCutscene; } set {_inCutscene = value; } }

	public float CurrentTimeLeft {
		get { return (dayTimeStart + dayLengthInSecs) - Time.time; }
	}

	public bool IsLate {
		get { return CurrentTimeLeft < 0; }
	}

	protected string CurrentTimeToString {
		get {
			TimeSpan t = TimeSpan.FromSeconds( CurrentTimeLeft );
//			int fastSecs = (int) ((t.Milliseconds/10) * (100/60));
			return string.Format("{0:D2}h:{1:D2}m", 
//			                     t.Hours, 
			                     t.Minutes,
			                     t.Seconds
//			                     fastSecs // To make it look like actual seconds (0-60 scale)
			                     );
		}
	}

	protected bool _finalTimeRecorded = false; // Kind of ugly way to avoid that the _finalTime is set continuously once player is late
	protected string _finalTime = "";

	// Use this for initialization
	void Start () {
//		Toolbox touch = Toolbox.Instance;


		AppearanceGameState.INSTANCE = this;
		dayTimeStart = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (IsLate) {
			if (!_finalTimeRecorded) {
				_finalTime = CurrentTimeToString;
				_finalTimeRecorded = true;
			}

			Toolbox.Instance.gameState.HenryCameHome = HENRY_CAME_HOME.LATE;

			//Toolbox.Instance.levelController.ReloadCurrent(2.0f);
			Toolbox.Instance.levelController.Load(LevelController.TABLE, 5.0f);
		}
	}

	void OnGUI() {
		if (!IsLate) {
			int timerWidth = 200;
			Rect pos = new Rect((Screen.width/2)-(timerWidth/2), Screen.height/5, timerWidth, 20);
			GUI.Box(pos, "Time passed: "+CurrentTimeToString);
		} else {
			int gameOverBoxWidth = Screen.width/2;
			Rect centerPos = new Rect((Screen.width/2)-(gameOverBoxWidth/2), Screen.height/2, gameOverBoxWidth, 40);
			GUI.Box(centerPos, "You went home too late, and your wife became suspicious! \n Time Spent: "+_finalTime);
		}
	}

	public void SpendTime(float time) {
		dayTimeStart -= time;  // Passed time is calculated based on (NOW - dayTimeStart), so push dayTimeStart back to increase passed time
	}
}
