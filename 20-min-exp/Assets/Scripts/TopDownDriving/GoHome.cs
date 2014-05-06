using UnityEngine;
using System.Collections;

public class GoHome : InvokableAction {

	public float successBufferInRealTimeSecs; // Buffer which allows the player go home a little early

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
			if (AppearanceGameState.INSTANCE.CurrentTimeLeft > 0 &&
			    AppearanceGameState.INSTANCE.CurrentTimeLeft < successBufferInRealTimeSecs)
			{
#if DEBUG
				Debug.Log ("Waifu not suspect!");
#endif
				Toolbox.Instance.gameState.HenryCameHome = HENRY_CAME_HOME.ON_TIME;

				int today = Toolbox.Instance.gameState.DayCounter;
				if (today == GameState.APPEARANCES_DAY_1 || today == GameState.APPEARANCES_DAY_2) {
//				if (LevelLoader.Status == LoadStatus.NotLoading) {
					Toolbox.Instance.levelController.Load(LevelController.TABLE, 2.0f);
				} else if (today == GameState.APPEARANCES_DAY_3) {
					Toolbox.Instance.levelController.Load(LevelController.SELL_STUFF, 2.0f);
				} else {
					Toolbox.Instance.levelController.Load(LevelController.TABLE, 2.0f);
				}
			}
			// Player is too early
			else if (AppearanceGameState.INSTANCE.CurrentTimeLeft > successBufferInRealTimeSecs)
			{
				Toolbox.Instance.gameState.HenryCameHome = HENRY_CAME_HOME.EARLY;

				StartCoroutine(Camera.main.ShowCenterText("You went home too early! Your wife will suspect something is wrong.", () => {
					Toolbox.Instance.levelController.Load(LevelController.TABLE);
//					Toolbox.Instance.levelController.Load(LevelController.APPEARANCES, 2.0f);
				}));
#if DEBUG
				Debug.Log ("You went home too early! Your wife will suspect something is wrong.");
#endif
			}
		}
	}

	protected float colorBump = 0.3f;
	public override void OnTriggerEnter(Collider collider) {
		if (!collider.gameObject.name.Equals ("Follower"))
			return;

		base.OnTriggerEnter(collider);
	
		var structure = this.transform.parent.transform.parent;
		foreach (Transform child in structure.transform) {
			foreach (Material mat in child.renderer.materials) {
				mat.color = new Color(mat.color.r+colorBump, mat.color.g+colorBump, mat.color.b+colorBump, mat.color.a);
			}
		}


	}

	public override void OnTriggerExit(Collider collider) {
		if (!collider.gameObject.name.Equals ("Follower"))
			return;

		base.OnTriggerExit(collider);

		var structure = this.transform.parent.transform.parent;
		foreach (Transform child in structure.transform) {
			foreach (Material mat in child.renderer.materials) {
				mat.color = new Color(mat.color.r-colorBump, mat.color.g-colorBump, mat.color.b-colorBump, mat.color.a);
			}
		}
	}


}
