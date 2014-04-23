using UnityEditor;
using UnityEngine;
using System.Collections;

public class WifeAgent : ConversationAgent {
    protected override void StopConversation() {
        base.StopConversation();
		int today = Toolbox.Instance.gameState.DayCounter;
		if (today == GameState.FIRING_DAY_AFTERNOON ||
		    today == GameState.APPEARANCES_DAY_1 ||
		    today == GameState.APPEARANCES_DAY_2)
		{
            Toolbox.Instance.gameState.DayCounter++;
            Toolbox.Instance.levelController.Load (LevelController.APPEARANCES);
		} else if (today == GameState.APPEARANCES_DAY_3) {
            Toolbox.Instance.gameState.DayCounter++;
            Toolbox.Instance.levelController.Load (LevelController.SELL_STUFF);
		} else {
        	Toolbox.Instance.levelController.LoadNext(10);
		}
    }
}
