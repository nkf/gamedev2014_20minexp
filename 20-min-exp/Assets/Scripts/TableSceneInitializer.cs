using UnityEngine;
using System.Collections;

public class TableSceneInitializer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int today = Toolbox.Instance.gameState.DayCounter;

		if (today == GameState.REGULAR_DAY)
			Debug.Log("Regular day");
		else if (today == GameState.FIRING_DAY_MORNING)
			Debug.Log("Firing day morning");
		else if (today == GameState.FIRING_DAY_AFTERNOON)
			Debug.Log("Firing day afternoon");
		else if (today == GameState.APPEARANCES_DAY_1)
			Debug.Log("Appearences day 1");
		else if (today == GameState.APPEARANCES_DAY_2)
			Debug.Log("Appearences day 2");
		else if (today == GameState.APPEARANCES_DAY_3) {
			Debug.Log("Appearences day 3");
			// TODO: Load Sell stuff?
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
