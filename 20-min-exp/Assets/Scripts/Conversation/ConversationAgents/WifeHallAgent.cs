using UnityEngine;
using System.Collections;

public class WifeHallAgent : ConversationAgent {

	public static WifeHallAgent JANE;
	
	protected void Awake() {
		WifeHallAgent.JANE = this;
	}

	protected override void StopConversation() {
		base.StopConversation();
		HallMovement.WifeSatisfied = true;
	}
}
