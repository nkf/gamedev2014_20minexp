using UnityEngine;
using System.Collections;

public class Sellable : Selectable {

    public int SellPrice = 500;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public override void Select() {
        Toolbox.Instance.gameState.MoneyCounter += SellPrice;
        Destroy(gameObject);
    }
}
