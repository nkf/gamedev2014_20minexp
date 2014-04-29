using UnityEngine;
using System.Collections;

public class TableSceneInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		Screen.lockCursor = true;

		int today = Toolbox.Instance.gameState.DayCounter;

		if (today == GameState.REGULAR_DAY)
			RegularDayInit();
		else if (today == GameState.FIRING_DAY_MORNING)
			FiringDayMorningInit();
		else if (today == GameState.FIRING_DAY_AFTERNOON)
			FiringDayAfternoon();
		else if (today == GameState.APPEARANCES_DAY_1)
			Appearances1Init();
		else if (today == GameState.APPEARANCES_DAY_2)
			Appearances2Init();
		else if (today == GameState.APPEARANCES_DAY_3) {
			Appearances3Init();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	protected void RegularDayInit() {
#if DEBUG
		Debug.Log("Regular day");
#endif
		WifeAgent.JANE.GetComponent<WifeAgent>()._conversationPath = "regularDayConvo";
		StaticCamera.STATIC_CAMERA.GetComponent<Animator>().enabled = false; //DISI IS HOW YOU DESABLE ANIMATORZ
	}

	protected void FiringDayMorningInit() {
#if DEBUG
		Debug.Log("Firing day morning");
#endif
		StaticCamera.STATIC_CAMERA.camera.SwitchTo();
		WifeAgent.JANE.GetComponent<WifeAgent>()._conversationPath = "firingDayConvo";
	}

	protected void FiringDayAfternoon() {
#if DEBUG
		Debug.Log("Firing day afternoon");
#endif	
		StaticCamera.STATIC_CAMERA.camera.SwitchTo();
		WifeAgent.JANE.GetComponent<WifeAgent>()._conversationPath = "firingNightConvo";
	}

	protected void Appearances1Init() {
#if DEBUG
		Debug.Log("Appearences day 1");
#endif
		StaticCamera.STATIC_CAMERA.camera.SwitchTo();
		WifeAgent.JANE.GetComponent<WifeAgent>()._conversationPath = "app1";
	}

	protected void Appearances2Init() {
#if DEBUG
		Debug.Log("Appearences day 2");
#endif
		StaticCamera.STATIC_CAMERA.camera.SwitchTo();
		WifeAgent.JANE.GetComponent<WifeAgent>()._conversationPath = "app2";
	}

	protected void Appearances3Init() {
#if DEBUG
		Debug.Log("Appearences day 3");
#endif
		StaticCamera.STATIC_CAMERA.camera.SwitchTo();

		// Disable Jane because homeful guy is lonely
		foreach (Transform child in WifeAgent.JANE.transform)
		{
			if (child.name.Equals("Wife")) {
				foreach (Transform child2 in child.transform)
					child2.renderer.enabled = false;
			}
		}

		// Disable Geoff because same reasons
		foreach (Transform child in KidAgent.GEOFF.transform)
		{
			if (child.name.Equals("Mesh")) {
				foreach (Transform child2 in child.transform)
					child2.renderer.enabled = false;
			}
		}

		// TODO: Load Sell stuff, but not through the wife script this time? 
	}
}
