using UnityEngine;
using System.Collections;

public class WifeAgent : ConversationAgent {
    public GameObject DinnerCamera;
    protected override void StopConversation() {
        base.StopConversation();
        var animator = DinnerCamera.GetComponent<Animator>();
        animator.Play("TransitionOutOfRoom");

    }
}
