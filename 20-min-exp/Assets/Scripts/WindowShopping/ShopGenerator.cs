using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Collections;
using Random = System.Random;

public class ShopGenerator : MonoBehaviour {
    public GameObject[] Shops;
    public int RandomSeed;
    private Random _rng;
    private Vector3 _currentPos;
	void Start () {
        _rng = new Random(RandomSeed);
	    _currentPos = transform.position;
	    for (int i = 0; i < 100; i++) {
	        CreateNextShop();
	    }
	}

    private int _shopId = 0;
    
    private void CreateNextShop() {
        _currentPos.x += 15;
        CreateShopTile(_currentPos);
    }
    private void CreateShopTile(Vector3 pos) {
        var go = Shops[_rng.Next(0, Shops.Length)];
        var shop = (GameObject)Instantiate(go);
        shop.transform.position = pos;
        shop.transform.parent = transform;
        shop.name = "#" + (_shopId++) + " shop (" + go.name + ")";
    }
    

    // Update is called once per frame
	void Update () {
	
	}
}
