using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections;

public class CarSpawn : MonoBehaviour {

    private static readonly List<CarSpawn> All = new List<CarSpawn>();
    void Awake() {
        All.Add(this);
    }
    private enum Pattern {
        Free, Taken, Main
    }
    private static Pattern[] _spawnPattern; 
    private static void CreateSpawnPattern() {
        if (_spawnPattern != null) return;
        _spawnPattern = new Pattern[All.Count];
        var cap = (int)Mathf.Floor(ParkingConfiguration.SpawnPct*All.Count);
        for (int i = 0; i < All.Count; i++) {
            if(i < cap) _spawnPattern[i] = Pattern.Taken;
            if(i == cap) _spawnPattern[i] = Pattern.Main;
            if(i > cap) _spawnPattern[i] = Pattern.Free;
        }
        _spawnPattern.Shuffle();
    }

    private static int _index;
    // Use this for initialization
	void Start () {
        if(_carPaths == null) _carPaths = ResourceUtil.GetPrefabPaths("CarPrefabs");
	    CreateSpawnPattern();
	    var pattern = _spawnPattern[_index];
	    if (pattern == Pattern.Main) {
	        var playerName = Toolbox.Instance.gameState.DayCounter == 2 ? "" : ParkingConfiguration.MainName;
	        GetComponentInChildren<TextMesh>().text = playerName;
	    } else {
	        if(pattern == Pattern.Taken) SpawnRandomCar();
	        GetComponentInChildren<TextMesh>().text = ParkingConfiguration.GetRandomName();
	        GetComponent<BoxCollider>().enabled = false;
	    }
	    _index++;
	    if (_index >= _spawnPattern.Length) _index = 0;
	}
    private readonly System.Random _rng = new System.Random();
    private static string[] _carPaths;
    private void SpawnRandomCar() {
        var load = Resources.Load<GameObject>(_carPaths[_rng.Next(0, _carPaths.Length)]);
        var car = (GameObject)Instantiate(load);
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
