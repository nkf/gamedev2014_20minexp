using UnityEngine;
using System.Collections;

public class Buyable : Selectable {

    // Use this for initialization
    private void Start() {}

    // Update is called once per frame
    private void Update() {}

    public override void Select() {
        Destroy(gameObject);
    }

}