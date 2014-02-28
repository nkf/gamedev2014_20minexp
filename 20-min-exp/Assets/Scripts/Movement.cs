using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public float Speed;
    private GameObject _head;
	// Use this for initialization
	void Start () {
	    _head = GetComponentInChildren<MouseLook>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	    var h = Input.GetAxis("Horizontal") * Time.deltaTime * Speed;
        var v = Input.GetAxis("Vertical") * Time.deltaTime * Speed;
	    var a = Quaternion.Euler(_head.transform.eulerAngles);
	    var force = new Vector3(h, 0, v);
        rigidbody.AddForce(a * force);
	}
}
