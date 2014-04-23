using UnityEngine;
using System.Collections;

public class CarControl : MonoBehaviour {

    public float Speed;
    public float TurnFactor;
    void Update() {
        //var v = Input.GetAxis("Vertical");
        var h = MouseHorizontalPosition();
        var a = transform.localEulerAngles;
        var turn = Mathf.InverseLerp(0.5f, 3, rigidbody.velocity.magnitude) * h;
        Debug.Log(turn);
        a = new Vector3(a.x, a.y + turn, 0);
        transform.localEulerAngles = a;
        var rotation = transform.rotation;
        var force = rotation * Vector3.forward * Speed;
        if(Input.GetMouseButton(0))
            rigidbody.AddForce(force);
        else if(Input.GetMouseButton(1))
            rigidbody.AddForce(-force);
    }

    private float MouseHorizontalPosition() {
        var mp = Input.mousePosition.x;
        var t = Mathf.InverseLerp(0, Screen.width, mp);
        return Mathf.Lerp(-1, 1, t);
    }
}
