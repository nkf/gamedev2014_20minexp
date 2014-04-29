using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

    public static readonly List<Selectable> All = new List<Selectable>();
    void Awake() {
        All.Add(this);
    }
    void OnDestroy() {
        All.Remove(this);
    }
    public virtual void Select() {
#if DEBUG
        Debug.Log(gameObject.name);
#endif
		Destroy (gameObject);
    }
}
