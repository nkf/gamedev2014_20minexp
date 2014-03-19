
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


    private string[] _normalRoadSections;
    private string[] _exoticRoadSections;
    private Random _rng;
    private const int RNGSeed = 100;
    private readonly LinkedList<GameObject> _roadList = new LinkedList<GameObject>(); 
	void Start () {
	    var path = Application.dataPath + "/Resources/RoadPrefabs";
	    var roadPaths = Directory.GetFiles(path).Select(s => CleanPath(s)).Distinct();
	    _normalRoadSections = roadPaths.Where(s => s.Contains("[n]")).ToArray();
	    _exoticRoadSections = roadPaths.Where(s => !s.Contains("[n]")).ToArray();
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

    private string getExoticSection() {
        return _exoticRoadSections[ _rng.Next(0, _exoticRoadSections.Length) ];
    }
    private string getNormalSection() {
        return _normalRoadSections[ _rng.Next(0, _normalRoadSections.Length) ];
    }
    private enum RoadType { Normal, Exotic }
    private string getSection(RoadType type) {
        if (type == RoadType.Normal) return getNormalSection();
        if (type == RoadType.Exotic) return getExoticSection();
        return null;
    }

    private int roadID = 0;
    private float CreateRoadTile(Vector3 pos, RoadType type) {
        var load = Resources.Load<GameObject>(getSection(type));
        var road = (GameObject)Instantiate(load);
        road.transform.position = pos;
        road.transform.parent = transform;
        road.name = "#"+(roadID++)+" road ("+load.name+")";
        _roadList.AddLast(road);
        return road.GetComponent<BoxCollider>().bounds.size.z;
    }

    private static string CleanPath(string path) {
        path = new Uri(path).AbsolutePath;
        var start = path.IndexOf(@"RoadPrefabs");
        var end = path.IndexOf(@".prefab");
        path = path.Substring(start, end - start);
        return Uri.UnescapeDataString(path);
    }
}
