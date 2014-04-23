using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class ShopGenerator : MonoBehaviour {
    private string[] _shopPaths;
    public int RandomSeed;
    private Random _rng;
    private Vector3 _currentPos;
	void Start () {
	    _shopPaths = ResourceUtil.GetPrefabPaths("ShopPrefabs");
        _rng = new Random(RandomSeed);
	    _currentPos = transform.position;
	    for (int i = 0; i < 1000; i++) {
	        CreateNextShop();
	    }
	}

    private int _shopId = 0;
    
    private void CreateNextShop() {
        _currentPos.x += 15;
        CreateShopTile(_currentPos);
    }
    private void CreateShopTile(Vector3 pos) {
        var load = Resources.Load<GameObject>(_shopPaths[_rng.Next(0, _shopPaths.Length)]);
        var road = (GameObject)Instantiate(load);
        road.transform.position = pos;
        road.transform.parent = transform;
        road.name = "#" + (_shopId++) + " shop (" + load.name + ")";
    }
    

    // Update is called once per frame
	void Update () {
	
	}
}
