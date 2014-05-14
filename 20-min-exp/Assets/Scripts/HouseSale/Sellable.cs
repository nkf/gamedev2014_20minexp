using System.Linq;
using System.Net.NetworkInformation;
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
        if (All.Count(s => s is Sellable) == 1) EndAnimation.Play();
        Destroy(gameObject);
    }
}
