using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TimeWaste : InvokableAction {

	public int timeWasteInMins;
	public int timeWastePrice;
	public int window1x;
	public int window1y;
	public Shader target;

	// TODO: fix 
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
			AppearanceGameState.INSTANCE.SpendTime(timeWasteInMins);    // Spend time
		}
	}

	/**
	 * Modify this method in a subclass if you want
	 */
    protected Fader _fader;
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
//		if (!_actionAvailable)
//			return;
//
//		Rect pos1 = new Rect(2*(Screen.width/5), 2*(Screen.height/5), Screen.width/5, Screen.height/5);
//		Rect pos2 = new Rect(2*(Screen.width/5), 3*(Screen.height/5), Screen.width/5, Screen.height/20);
//
//		GUIHelpers.DrawQuad(pos1, Color.black);
//		GUI.Box(pos1, "\nJay's Billiards & Sports Bar \n\n You can spend an hour here for $1000");
//		GUI.Box(pos2, "Press ENTER to enter");
	}


	public override void OnTriggerEnter(Collider collider) {
		base.OnTriggerEnter(collider);

		var timeWaste = this.transform.parent;
		var houseModel = timeWaste.transform.parent;
		// Some reason the target field is till null at this point
		Material mat = houseModel.renderer.materials.FirstOrDefault(m => m.shader.name.Equals ("Outlined Diffuse"));

		if (mat != null) {
			StartCoroutine(_FadeIn(mat, 0.1f));
		}
	}

	public override void OnTriggerExit(Collider collider) {
		base.OnTriggerExit(collider);

		var timeWaste = this.transform.parent;
		var houseModel = timeWaste.transform.parent;

		// Some reason the  field is till null in the script.
		Material mat = houseModel.renderer.materials.FirstOrDefault(m => m.shader.name.Equals ("Outlined Diffuse"));

		if (mat != null) {
			StartCoroutine(_FadeOut(mat, 0.1f));
		}
	}



}
