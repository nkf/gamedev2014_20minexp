using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class CarSpawn : MonoBehaviour {

    public static readonly List<CarSpawn> All = new List<CarSpawn>();
    void Awake() {
        All.Add(this);
    }

    void OnDestroy() {
        All.Remove(this);
    }

    private static int _index;

    private TextMesh _name;
	void Start () {
	    _name = GetComponentsInChildren<TextMesh>().First(t => t.name == "Name");
	    var spot = ParkingConfiguration.GetSpotSpawn(_index);
	    if (spot.Taken) {
	        SpawnRandomCar();
	        collider.enabled = false;
	    }

	    if (spot.Name == Toolbox.Instance.gameState.CharacterName) { //CharacterName will be set on the 2nd day.
	        _name.text = "";
	    } else {
            GetComponent<ParkingCheck>().SetName(spot.Name);
            _name.text = spot.Name;
	        if (Toolbox.Instance.gameState.DayCounter == GameState.FIRING_DAY_MORNING) collider.enabled = false;
	    }
	    _index++;
	    if (_index >= All.Count) _index = 0;
	}
    private readonly System.Random _rng = new System.Random();
    private void SpawnRandomCar() {
        var carPrefabs = ParkingConfiguration.Cars;
        var go = carPrefabs[_rng.Next(0, carPrefabs.Length)];
        var car = (GameObject)Instantiate(go);
        var pos = transform.position;
        pos.y +=  0.7f;
        car.transform.position = pos;
        car.transform.parent = transform;
        car.transform.localEulerAngles += transform.localEulerAngles;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
