using UnityEngine;
using System.Collections;

public class WifeHallAgent : ConversationAgent {

	public static WifeHallAgent JANE;

	public string conversationPath2;
	
	protected void Awake() {
		WifeHallAgent.JANE = this;

		int today = Toolbox.Instance.gameState.DayCounter;
		if (today == GameState.REGULAR_DAY) {
			// Don't change the conversation path from the default
		} else if (today == GameState.FIRING_DAY_MORNING) {
			_conversationPath = conversationPath2;
		}
	}

	protected override void StopConversation() {
		base.StopConversation();
		HallMovement.WifeSatisfied = true;
	}
}
