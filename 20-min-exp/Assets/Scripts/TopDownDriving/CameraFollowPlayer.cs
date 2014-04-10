using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (AppearanceGameState.INSTANCE.InCutscene)
			return;

		Vector3 newPos = new Vector3(
			Car.PLAYER.transform.position.x,
			this.transform.position.y,
			Car.PLAYER.transform.position.z);

		this.transform.position = newPos;

		this.transform.rotation = Quaternion.Euler(
			90,
			Car.PLAYER.transform.rotation.eulerAngles.y,
			this.transform.rotation.z);
	}
}
