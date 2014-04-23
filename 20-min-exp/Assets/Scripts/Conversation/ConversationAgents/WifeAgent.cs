using UnityEditor;
using UnityEngine;
using System.Collections;

public class WifeAgent : ConversationAgent {

	public static GameObject JANE;

	protected void Awake() {
		WifeAgent.JANE = this.gameObject;
	}

    protected override void StopConversation() {
        base.StopConversation();
		int today = Toolbox.Instance.gameState.DayCounter;
		if (today == GameState.FIRING_DAY_AFTERNOON ||
		    today == GameState.APPEARANCES_DAY_1 ||
		    today == GameState.APPEARANCES_DAY_2)
		{
			Toolbox.Instance.levelController.Load (LevelController.APPEARANCES);
			Toolbox.Instance.gameState.DayCounter++;
		} else if (today == GameState.APPEARANCES_DAY_3) {
			Toolbox.Instance.levelController.Load (LevelController.SELL_STUFF);
			Toolbox.Instance.gameState.DayCounter++;
		} else {
        	Toolbox.Instance.levelController.LoadNext(10);
		}
    }
}
