using UnityEngine;
using System.Collections;

public class AnimationFunctions : MonoBehaviour {

    public GameObject Door1;
	// Use this for initialization
	void Start () {
	
	}
    public void OpenDoor1() {
        var ani = Door1.GetComponent<Animation>();
        ani.Play();
    }
}
