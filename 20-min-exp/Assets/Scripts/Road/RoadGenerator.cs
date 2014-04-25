
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
	    Camera.main.GetComponent<RoadController>().EnteredRoad += road => {
	        var firstRoad = _roadList.First.Value;
	        if (road != firstRoad) {
                _roadList.RemoveFirst();
                Destroy(firstRoad);
                SpawnRoad();
	        }
	    };
	}

    private float _z;
    private int _normalRoadCounter = 0;
    private int _normalRoadAmount = 5;

    private void SpawnRoad() {
        if(_normalRoadCounter == _normalRoadAmount) {
            _normalRoadCounter = 0;
            _normalRoadAmount = _rng.Next(3, 10);
            _z += CreateRoadTile(new Vector3(0, 0, _z), RoadType.Exotic);
        } else {
            _normalRoadCounter++;
            _z += CreateRoadTile(new Vector3(0, 0, _z), RoadType.Normal);
        }
    }

    private GameObject getExoticSection() {
        return ExoticRoads[ _rng.Next(0, ExoticRoads.Length) ];
    }
    private GameObject getNormalSection() {
        return NormalRoads[ _rng.Next(0, NormalRoads.Length) ];
    }
    private enum RoadType { Normal, Exotic }
    private GameObject getSection(RoadType type) {
        if (type == RoadType.Normal) return getNormalSection();
        if (type == RoadType.Exotic) return getExoticSection();
        return null;
    }

    private int roadID = 0;
    private float CreateRoadTile(Vector3 pos, RoadType type) {
        var g = getSection(type);
        var road = (GameObject)Instantiate(g);
        road.transform.position = pos;
        road.transform.parent = transform;
        road.name = "#"+(roadID++)+" road ("+g.name+")";
        _roadList.AddLast(road);
        return road.GetComponent<BoxCollider>().bounds.size.z;
    }

}
