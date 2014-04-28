using UnityEngine;
using System.Collections;



public class TimeWaste : InvokableAction {

	public int timeWasteInMins;
	public int timeWastePrice;
	public int window1x;
	public int window1y;

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
	 * Modify this method in a subclass if you want
	 */
    private Fader _fader;
	protected void PlayCutscene() {
		// Fade out and in to indicate time spent.
		AppearanceGameState.INSTANCE.InCutscene = true;
	    _fader = new Fader();
		// Fade to black
		StartCoroutine(
			_fader.FadeToBlack(
				2,
				() => StartCoroutine(_fader.FadeInFromBlack(2, () => OnEndCutscene())) // Just fade back in afterwards
			)
		);

	}

    void OnDestroy() {
        if(_fader != null) _fader.Clear();
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

		Rect pos1 = new Rect(2*(Screen.width/5), 2*(Screen.height/5), Screen.width/5, Screen.height/5);
		Rect pos2 = new Rect(2*(Screen.width/5), 3*(Screen.height/5), Screen.width/5, Screen.height/20);

		GUIHelpers.DrawQuad(pos1, Color.black);
		GUI.Box(pos1, "\nJay's Billiards & Sports Bar \n\n You can spend an hour here for $1000");
		GUI.Box(pos2, "Press ENTER to enter");
	}
}
