using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public class ParkingCheck : MonoBehaviour {

    private BoxCollider _box;
    private string _name;
	// Use this for initialization
	void Start () {
	    _box = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetName(string str) {
        _name = str;
    }

    void OnTriggerStay(Collider other) {
        var carBox = other.GetComponent<BoxCollider>();
        var corners = GetBoundingCorners(carBox, other.transform);
        var contained = corners.Aggregate(true, (acc, corner) => acc & _box.bounds.Contains(corner));
        if (contained) {
            if(_name != null) Toolbox.Instance.gameState.CharacterName = _name;
            Toolbox.Instance.levelController.Load(LevelController.OFFICE);
        }
    }

    private static IEnumerable<Vector3> GetBoundingCorners(BoxCollider box, Transform origin) {
        var rotation = origin.localRotation;
        var center = (rotation * box.center) + origin.position;
        var halfSize = box.size/2;
        return new [] {
            center + (rotation * new Vector3( halfSize.x, 0,  halfSize.z)),
            center + (rotation * new Vector3(-halfSize.x, 0,  halfSize.z)),
            center + (rotation * new Vector3( halfSize.x, 0, -halfSize.z)),
            center + (rotation * new Vector3(-halfSize.x, 0, -halfSize.z))
        };
    }
}
