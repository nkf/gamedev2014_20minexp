using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;
[RequireComponent(typeof(BoxCollider))]
public class SelectionEnabler : MonoBehaviour {

    private SideScrollController _car;
    private Selectable[] _selectables;
    private bool _disabled;
    void Start() {
        _selectables = transform.parent.GetComponentsInChildren<Selectable>();
        _car = GameObject.FindGameObjectWithTag("Player").GetComponent<SideScrollController>();

    }

    private void EnableSelection() {
        foreach (var selectable in _selectables) {
            if (selectable != null) {
                selectable.enabled = true;
            }
        }
        _disabled = false;
    }

    private void DisableSelection() {
        foreach (var selectable in _selectables) {
            if (selectable != null) {
                selectable.enabled = false;
            }
        }
        _disabled = true;
    }
    void OnTriggerStay(Collider other) {
        if (_car.Speed > 0.5f && !_disabled) DisableSelection();
        else if (_car.Speed <= 0.5f && _disabled) EnableSelection();
    }
    void OnTriggerExit(Collider other) {
        DisableSelection();
    }
}
