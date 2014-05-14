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
        SetMoney(today);
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

    private void SetMoney(int day) {
        var gs = Toolbox.Instance.gameState;
        if (day == GameState.FIRING_DAY_AFTERNOON) gs.MoneyCounter -= 25657;
        if (day == GameState.APPEARANCES_DAY_1) gs.MoneyCounter -= 37596;
        if (day == GameState.APPEARANCES_DAY_2) gs.MoneyCounter = 2596;
    }
}
