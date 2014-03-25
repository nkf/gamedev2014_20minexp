using UnityEngine;
using System.Collections;

public class HallAnimationFunctions : MonoBehaviour {

    public ConversationAgent Conversation1;
    // Use this for initialization
    void Start() {

    }

    public void StartConversation1() {
        Conversation1.StartConversation();
    }

	public void EndHallAnimation() {
		Toolbox.Instance.levelController.LoadNext();
	}

}
