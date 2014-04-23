using UnityEngine;
using System.Collections;

public class GoHome : InvokableAction {

	public float successBufferInSecs; // Buffer which allows the player go home a little early

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!_actionAvailable)
			return;

		if (Input.GetKeyDown(KeyCode.Return)) {
			_actionAvailable = false;
			Car.PLAYER.SetControls(false);

			// Player has returned home in time without suspiciating das waifu
			if (AppearanceGameState.INSTANCE.CurrentTime > AppearanceGameState.INSTANCE.dayLengthInSecs-successBufferInSecs &&
			    AppearanceGameState.INSTANCE.CurrentTime < AppearanceGameState.INSTANCE.dayLengthInSecs)
			{
//				int today = Toolbox.Instance.gameState.DayCounter;
//				if (today == GameState.APPEARANCES_DAY_1 || GameState.APPEARANCES_DAY_2) {
				if (LevelLoader.Status == LoadStatus.NotLoading) {
					Debug.Log ("Waifu not suspect!");
					Toolbox.Instance.levelController.Load(LevelController.TABLE, 2.0f);
				}
			}
			// Player is too early
			else if (AppearanceGameState.INSTANCE.CurrentTime < AppearanceGameState.INSTANCE.dayLengthInSecs-successBufferInSecs)
			{
				Debug.Log ("U WENT HOME EARLY! WAIFU SUSPECTU DESU!");
				Toolbox.Instance.levelController.ReloadCurrent(2.0f);
			}
			// Actually, this following case is already handled by AppearanceGameState
			// Player is late.
//			else if (AppearanceGameState.INSTANCE.CurrentTime > AppearanceGameState.INSTANCE.dayLengthInSecs)
//			{
//				Debug.Log ("U LATE! WAIFU THINK U AFFAIR!");
//			}
		}
	}

	void OnGUI() {
		if (!_actionAvailable)
			return;
		
		int pos1Height = 100;
		int posWidth   = 200;
		Rect pos1 = new Rect(Screen.width/5, Screen.height/5, posWidth, pos1Height);
		Rect pos2 = new Rect(Screen.width/5, (Screen.height/5)+pos1Height, posWidth, 20);
		
		GUIHelpers.DrawQuad(pos1, Color.black);
		GUI.Box(pos1, "Go Home...");
		GUI.Box(pos2, "Press Enter, to go to waifu.");
	}
}
