using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour {
    
    private GameObject _roadBelow;
    private Bounds _roadBounds;
	// Use this for initialization
	void Start () {

	}

    public float Speed = 50;
    public float SteeringSpeed = 0.5f;
    public float SwayFactor = 0.02f;
    public float EdgeSlowDownThreshold = 3f;
    public float EdgePadding = 1f;
    private float _slowDownFactor = 1;
	// Update is called once per frame
	void Update () {
	    var p = transform.position;
	    var hInput = Input.GetAxis("Horizontal");
	    var move = hInput * SteeringSpeed + (Random.value*SwayFactor);
	    //Calculate edge slowdown
	    var edgeDis = DistanceToEdge();
	    _slowDownFactor = Mathf.InverseLerp(0, EdgeSlowDownThreshold, edgeDis);
        //if we are going too close to the edge AND are on course towards the edge.
	    if (edgeDis < EdgePadding && Mathf.Sign(transform.position.x) == Mathf.Sign(move)) move = 0;
        //apply
	    p.x += move * _slowDownFactor;
        p.z += Speed * Time.deltaTime * _slowDownFactor;
	    transform.position = p;
	    
	}

    void FixedUpdate() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit)) {
            var road = hit.collider.gameObject;
            if (road != _roadBelow) EnteredRoad(road);
            _roadBelow = road;
            _roadBounds = _roadBelow.GetComponent<BoxCollider>().bounds;
        }
    }

    //!!!!! THIS ONLY WORKS BECAUSE ROAD IS CENTERED @ x = 0 !!!!
    private float DistanceToEdge() {
         return Mathf.Abs(Mathf.Abs(transform.position.x) - _roadBounds.extents.x);
    }

    public event Action<GameObject> EnteredRoad;
    protected virtual void OnEnteredRoad(GameObject obj) {
        Action<GameObject> handler = EnteredRoad;
        if (handler != null) handler(obj);
    }
}
