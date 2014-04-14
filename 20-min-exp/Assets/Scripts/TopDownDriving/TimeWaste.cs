using UnityEngine;
using System.Collections;

public class TimeWaste : InvokableAction {

	public int timeWasteInMins;
	public int timeWastePrice;

	protected float timeWasteInSecs    { get { return timeWasteInMins * 60; } }

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!_actionAvailable)
			return;


		if (Input.GetKeyDown(KeyCode.Return)) {
			PlayCutscene();

			// Diable action and car controls
			_actionAvailable = false;
			Car.PLAYER.SetControls(false);

			Toolbox.Instance.gameState.MoneyCounter -= timeWastePrice;  // Pay for what ever this is.
			AppearanceGameState.INSTANCE.SpendTime(timeWasteInSecs);    // Spend time
		}
	}

	/**
	 * Modify this method in a subclass or some shit
	 */
	protected void PlayCutscene() {
		// Fade out and in to indicate time spent.
		AppearanceGameState.INSTANCE.InCutscene = true;

		// Fade to black
		StartCoroutine(
			Camera.main.FadeToBlack(
				2,
				() => StartCoroutine(Camera.main.FadeInFromBlack(2, () => OnEndCutscene())) // Just fade back in afterwards
			)
		);

	}
	/**
	 * Also modify this, I don't care.
	 */
	protected void OnEndCutscene() {
		AppearanceGameState.INSTANCE.InCutscene = false;

		// Reenable car controls. Mucho importante
		Car.PLAYER.SetControls(true);
	}

	void OnGUI() {
		if (!_actionAvailable)
			return;

		int pos1Height = 100;
		int posWidth   = 150;
		Rect pos1 = new Rect(Screen.width/5, Screen.height/5, posWidth, pos1Height);
		Rect pos2 = new Rect(Screen.width/5, (Screen.height/5)+pos1Height, posWidth, 20);

		GUIHelpers.DrawQuad(pos1, Color.black);
		GUI.Box(pos1, "Hej, MOSMF \n mullimahuthut \n whutevs");
		GUI.Box(pos2, "Press a key, to get shitefaced");
	}
}
