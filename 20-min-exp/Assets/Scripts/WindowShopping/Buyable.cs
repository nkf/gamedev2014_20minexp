using UnityEngine;
using System.Collections;

public class Buyable : Selectable {

    public int BuyPrice;
    // Use this for initialization
    private void Start() {}

    // Update is called once per frame
    private void Update() {}

    public override void Select() {
        Toolbox.Instance.gameState.MoneyCounter -= BuyPrice;
        Destroy(gameObject);
        if (!SideScrollController.HasBoughtStuff) {
            SideScrollController.CONTROLLER.StartTime = Time.time;
            SideScrollController.HasBoughtStuff = true;
        }
    }

}