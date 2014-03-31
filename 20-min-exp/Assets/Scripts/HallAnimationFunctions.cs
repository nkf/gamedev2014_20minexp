using UnityEngine;
using System.Collections;

public class HallAnimationFunctions : MonoBehaviour {

	public GameObject door;
    public ConversationAgent conversationKid;

    // Use this for initialization
    void Start() {

    }

    public void StartConversation1() {
        conversationKid.StartConversation();
    }

	public void OpenDoorNew() {
		var ani = door.GetComponent<Animation>();
		ani.Play();
	}

	public void EndHallAnimation() {
		Toolbox.Instance.levelController.LoadNext();
	}

}
