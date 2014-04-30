using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class RoadController : MonoBehaviour {
    
    private GameObject _roadBelow;
    private Bounds _roadBounds;

    public int RoadsPassed { get; private set; }
    public int EndAtRoadNumber;
    void Start() {
        EnteredRoad += o => {
            if (RoadsPassed >= EndAtRoadNumber) {
                EndScene();
            }
        };
    }

    public float Acceleration     = 1f;
    public float DeAcceleration   = 1f;
    public float MinSpeed         = 40;
    public float MaxSpeed         = 100;
    public float SteeringSpeed    = 0.5f;
    public float SwayFactor       = 0.02f;
    public float EdgeSlowDownThreshold = 3f;
    public float EdgePadding      = 1f;
    private float _slowDownFactor = 1;
    private float _speed;
    private float _minSpeed = 0;
    private bool _cruiseControl = false;

    // Update is called once per frame
	void Update () {
	    var p = transform.position;
	    var hInput = Input.GetAxis("Horizontal");
	    var vInput = Input.GetAxis("Vertical");
//	    var hInput = MouseHorizontalPosition();
//	    if (Input.GetMouseButton(0))
		if (vInput > 0)
			_speed += Acceleration * Mathf.InverseLerp(-1,20,_speed) * Time.deltaTime;
	    else if (!_cruiseControl)
	        _speed -= DeAcceleration * Time.deltaTime * 0.1f;
	    else {
            _speed -= DeAcceleration * Time.deltaTime;
	    }
	    _speed = Mathf.Clamp(_speed, _minSpeed, MaxSpeed);
	    var move = hInput * SteeringSpeed + GetSway();
	    move *= Mathf.InverseLerp(5, 30, _speed); //prevent sideways movement when no forward movement
	    
        //Calculate cruisecontrol level 
	    if (!_cruiseControl) {
	        if (_speed >= MinSpeed*1.5f) {
	            EnableCruiseControl();
	        }
	    }

		//Calculate edge slowdown
	    var edgeDis = DistanceToEdge();
	    _slowDownFactor = Mathf.InverseLerp(0, EdgeSlowDownThreshold, edgeDis);
        
        //dramatic camara shake if you drive at the curb :D
	    if (_slowDownFactor < 0.5f && _speed > 3) {
	        StartCoroutine(Camera.main.Shake(2, 0.05f, 0.5f));
	    }

		//if we are going too close to the edge AND are on course towards the edge.
	    if (edgeDis < EdgePadding && Mathf.Sign(transform.position.x) == Mathf.Sign(move))
			move = 0;

		//apply
	    p.x += move * _slowDownFactor;
        p.z += _speed * Time.deltaTime * _slowDownFactor;
	    transform.position = p;
	}

    private int _nextSwayChange = 100;
    private int _swayIndex = 0;
    private int _currentSway = 1;
    private float GetSway() {
        _swayIndex++;
        if (_swayIndex >= _nextSwayChange) {
            _nextSwayChange = Random.Range(10, 100);
            _swayIndex = 0;
            _currentSway = -_currentSway;
        }
        return _currentSway * SwayFactor * Mathf.InverseLerp(MinSpeed, MaxSpeed, _speed);
    }

    public void EnableCruiseControl() {
        _minSpeed = MinSpeed;
        _cruiseControl = true;
        Debug.Log("CRUISECONTROL ENGAGED!");
    }

    private float MouseHorizontalPosition() {
        var mp = Input.mousePosition.x;
        var t = Mathf.InverseLerp(0, Screen.width, mp);
        return Mathf.Lerp(-1, 1, t);
    }

    void EndScene() {
        Toolbox.Instance.levelController.Load(LevelController.PARKING);
    }

    void FixedUpdate() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit)) {
            var road = hit.collider.gameObject;
            if (road != _roadBelow) OnEnteredRoad(road);
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
        RoadsPassed++;
        if (EnteredRoad != null) EnteredRoad(obj);
    }
}
