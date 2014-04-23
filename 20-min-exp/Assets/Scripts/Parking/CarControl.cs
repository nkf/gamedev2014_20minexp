using UnityEngine;
using System.Collections;

public class CarControl : MonoBehaviour {

    public float Speed = 2;
    void Update() {
        //var v = Input.GetAxis("Vertical");
        var h = MouseHorizontalPosition();
        var a = transform.localEulerAngles;
        var turn = Mathf.InverseLerp(1f, 5, rigidbody.velocity.magnitude) * h;
        a = new Vector3(a.x, a.y + turn, 0);
        transform.localEulerAngles = a;
        var rotation = transform.rotation;
        var force = rotation * Vector3.forward * Speed;
        if(Input.GetMouseButton(0)) {
			Debug.Log ("Forward...");
            rigidbody.AddForce(force);
		}
        else if(Input.GetMouseButton(1)) {
			Debug.Log ("Backward...");
            rigidbody.AddForce(-force);
		}

        if (Input.GetKey(KeyCode.K) && !nigga) {
            nigga = true;
            StartCoroutine(Camera.main.FadeToBlack(2, () => Debug.Log("oookay")));
        }
    }

    private bool nigga = false;
    private float MouseHorizontalPosition() {
        var mp = Input.mousePosition.x;
        var t = Mathf.InverseLerp(0, Screen.width, mp);
        return Mathf.Lerp(-1, 1, t);
    }
}
