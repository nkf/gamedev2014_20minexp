using UnityEngine;
using System.Collections;

public class KidAgent : ConversationAgent {
    
	public static GameObject GEOFF;

	public void Awake() {
		KidAgent.GEOFF = this.gameObject;
	}

	public GameObject HallCamera;
    
	protected override void StopConversation() {
        base.StopConversation();
        var animator = HallCamera.GetComponent<Animator>();
        animator.Play("GoDownHall2");
    }
}
