
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections;
using System.Linq;
using Random = System.Random;


public class RoadGenerator : MonoBehaviour {

    public int RoadBufferSize;
    public GameObject[] StartRoads;
    public int StartRoadAmount = 10;
    public GameObject[] NormalRoads;
    public GameObject[] ExoticRoads;
    private Random _rng;
    private const int RNGSeed = 100;
    private readonly LinkedList<GameObject> _roadList = new LinkedList<GameObject>(); 
	void Start () {
        _rng = new Random(RNGSeed);
	    for (int i = 0; i < RoadBufferSize; i++) {
	        SpawnRoad();
	    }
	    var rc = Camera.main.GetComponent<RoadController>();
	    rc.EnteredRoad += road => {
	        var firstRoad = _roadList.First.Value;
	        if (road != firstRoad) {
                _roadList.RemoveFirst();
                Destroy(firstRoad);
                SpawnRoad();
	        }
	    };
	    Action<GameObject> handler = null;
        handler = road => {
            if(rc.RoadsPassed > StartRoadAmount) {
                rc.EnableCruiseControl();
                rc.EnteredRoad -= handler;
            }
        };
	    rc.EnteredRoad += handler;
	}

    private float _z;
    private int _normalRoadCounter = 0;
    private int _normalRoadAmount = 5;
    private int _startRoadCounter = 0;
    

    private void SpawnRoad() {
        if (_startRoadCounter < StartRoadAmount) {
            _startRoadCounter++;
            _z += CreateRoadTile(new Vector3(0, 0, _z), RoadType.Start);
        } else if(_normalRoadCounter == _normalRoadAmount) {
            _normalRoadCounter = 0;
            _normalRoadAmount = _rng.Next(3, 10);
            _z += CreateRoadTile(new Vector3(0, 0, _z), RoadType.Exotic);
        } else {
            _normalRoadCounter++;
            _z += CreateRoadTile(new Vector3(0, 0, _z), RoadType.Normal);
        }
    }

    private GameObject GetRandomSection(GameObject[] from) {
        return from[_rng.Next(0, from.Length)];
    }
    private enum RoadType { Start, Normal, Exotic }
    private GameObject GetSection(RoadType type) {
        if (type == RoadType.Start)  return GetRandomSection(StartRoads);
        if (type == RoadType.Normal) return GetRandomSection(NormalRoads);
        if (type == RoadType.Exotic) return GetRandomSection(ExoticRoads);
        return null;
    }

    private int roadID = 0;
    private float CreateRoadTile(Vector3 pos, RoadType type) {
        var go = GetSection(type);
        var road = (GameObject)Instantiate(go);
        road.transform.position = pos;
        road.transform.parent = transform;
        road.name = "#"+(roadID++)+" road ("+go.name+")";
        _roadList.AddLast(road);
        return road.GetComponent<BoxCollider>().bounds.size.z;
    }

}
