using UnityEngine;
using System.Collections;

public class FPS_Movement : MonoBehaviour {

    public float Speed;
    public float SlowDownThreshold = 3;
    private GameObject _head;
	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		Screen.lockCursor = true;
	    _head = GetComponentInChildren<MouseLook>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
	    var a = Quaternion.Euler(_head.transform.eulerAngles);
        var force = new Vector3(Velocity(h), 0, Velocity(v));
        rigidbody.AddForce(a * force);
        //less slide more rest
	    if (Mathf.Abs(h) + Mathf.Abs(v) < 0.2) {
	        var velocity = rigidbody.velocity;
	        if (velocity.magnitude < SlowDownThreshold && velocity.magnitude > 0.1f) {
	            rigidbody.velocity = SubToZero(velocity, 0.4f);
	        }  
	    }
	}

    float Velocity(float val) {
        return val * Time.deltaTime * Speed;
    }

    Vector3 SubToZero(Vector3 v, float a) {
        var x = Mathf.Max(v.x - a, 0);
        var y = Mathf.Max(v.y - a, 0);
        var z = Mathf.Max(v.z - a, 0);
        return new Vector3(x,y,z);
    }
}
