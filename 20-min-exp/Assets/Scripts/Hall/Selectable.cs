using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {

    public static readonly List<Selectable> All = new List<Selectable>();
    void Awake() {
        All.Add(this);
    }
    void OnDestroy() {
        All.Remove(this);
    }
    public void Select() {
        Destroy(gameObject);
    }
}
