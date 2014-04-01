using UnityEngine;
using System.Collections;

public class DinnerAnimationFunctions : MonoBehaviour {

    public ConversationAgent Conversation1;
    
	// Use this for initialization
	void Start () {
	
	}

    public void StartConversation1() {
        Conversation1.StartConversation();
    }

}
