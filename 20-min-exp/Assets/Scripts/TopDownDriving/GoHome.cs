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
				Debug.Log ("Waifu not suspect!");

				int today = Toolbox.Instance.gameState.DayCounter;
				if (today == GameState.APPEARANCES_DAY_1 || today == GameState.APPEARANCES_DAY_2) {
//				if (LevelLoader.Status == LoadStatus.NotLoading) {
					Toolbox.Instance.levelController.Load(LevelController.TABLE, 2.0f);
				} else if (today == GameState.APPEARANCES_DAY_3) {
					Toolbox.Instance.levelController.Load(LevelController.SELL_STUFF, 2.0f);
				} else {
					Toolbox.Instance.levelController.Load(LevelController.TABLE, 2.0f);
				//	StartCoroutine(Camera.main.ShowCenterText("Faggot.", () => {
				//		Toolbox.Instance.levelController.Load(LevelController.APPEARANCES, 2.0f);
			//		}));
				}
			}
			// Player is too early
			else if (AppearanceGameState.INSTANCE.CurrentTime < AppearanceGameState.INSTANCE.dayLengthInSecs-successBufferInSecs)
			{
				StartCoroutine(Camera.main.ShowCenterText("You went home too early! Your wife will suspect something is wrong.", () => {
					Toolbox.Instance.levelController.Load(LevelController.APPEARANCES, 2.0f);
				}));
				Debug.Log ("You went home too early! Your wife will suspect something is wrong.");

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

		Rect pos1 = new Rect(2*(Screen.width/5), 2*(Screen.height/5), Screen.width/5, Screen.height/5);
		Rect pos2 = new Rect(2*(Screen.width/5), 3*(Screen.height/5), Screen.width/5, Screen.height/20);
		
		GUIHelpers.DrawQuad(pos1, Color.black);
		GUI.Box(pos1, "Go Home...");
		GUI.Box(pos2, "Press ENTER to return to your family");
	}
}
