using UnityEngine;
using System.Collections;

public class DinnerAnimationFunctions : MonoBehaviour {

	public static GameObject DINNER_CAMERA;

	public void Awake() {
		DINNER_CAMERA = this.gameObject;
	}

	// Use this for initialization
	void Start () {
	
	}

    public void StartConversation1() {
		WifeAgent.JANE.GetComponent<WifeAgent>().StartConversation();
    }

}
