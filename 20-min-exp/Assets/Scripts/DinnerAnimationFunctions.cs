using UnityEngine;
using System.Collections;

public class DinnerAnimationFunctions : MonoBehaviour {

    public GameObject Door1;
    public ConversationAgent Conversation1;
    public Camera Camera1;
	// Use this for initialization
	void Start () {
	
	}

    public void StartConversation1() {
        Conversation1.StartConversation();
    }

    public void OpenDoor1() {
        var ani = Door1.GetComponent<Animation>();
        ani.Play();
    }

    public void SwitchToCamera1() {
        Camera1.SwitchTo();
        Camera1.GetComponent<Animator>().Play("GoDownHall1");
    }
}
