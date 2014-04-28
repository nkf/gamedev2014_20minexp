using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

public enum SelectionType {
	MOUSE, MIDDLE_SCREEN
}

public class LookSelect : MonoBehaviour {

    

    public float SelectionDistance;
    public Shader Target;
    public SelectionType SelectionMethod;
    public float FadeInTime = 0.5f;
    public float FadeOutTime = 0.25f;
    private const string Outline = "_OutlineColor";
    private bool _fadingIn;
    private readonly List<Material> _fadingOut = new List<Material>();
    private Material _current;
    private Selectable _currentSelection;
	void Update () {
	    Ray ray;
	    if (SelectionMethod == SelectionType.MIDDLE_SCREEN) {
	        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2f, Screen.height/2f));
	    } else { //Selection.Mouse
	        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    }
        RaycastHit hit;
	    if (Physics.Raycast(ray, out hit, SelectionDistance)) {
	        var material = hit.collider.renderer.materials.FirstOrDefault(m => m.shader == Target);
            var selectable = hit.collider.GetComponent<Selectable>();
	        if (material != _current && selectable != null && selectable.enabled) {
	            if (_current != null) FadeOut(_current, FadeOutTime);

	            _current = material;
	            _currentSelection = selectable;
	            _fadingIn = true;
	            FadeIn(material, FadeInTime);
	        }
	    } else if(_current != null) {
	        _fadingIn = false;
	        FadeOut(_current, FadeOutTime);
	        _current = null;
	        _currentSelection = null;
	    }
	    if (Input.GetMouseButton(0)) {
	        if(_currentSelection != null) _currentSelection.Select();
	    }
	}

    private void FadeIn(Material m, float time) {
        StartCoroutine(_FadeIn(m, time));
    }

    private void FadeOut(Material m, float time) {
        if (_fadingOut.Contains(m)) return;
        _fadingOut.Add(m);
        StartCoroutine(_FadeOut(m, time));
    }

    private IEnumerator _FadeOut(Material m, float time) {
        var c = m.GetColor(Outline);
        var startAlpha = c.a;
        var start = Time.time;
        var end = start + time;
        while(Time.time <= end) {
            var progress = Mathf.InverseLerp(start, end, Time.time);
            var alpha = Mathf.Lerp(startAlpha, 0, progress);
            m.SetColor(Outline, SetAlpha(c, alpha));
            yield return new WaitForEndOfFrame();
        }
        m.SetColor(Outline, SetAlpha(c, 0));
        _fadingOut.Remove(m);
    }

    private IEnumerator _FadeIn(Material m, float time) {
        var c = m.GetColor(Outline);
        var startAlpha = c.a;
        var start = Time.time;
        var end = start + time;
        while(Time.time <= end && _fadingIn) {
            var progress = Mathf.InverseLerp(start, end, Time.time);
            var alpha = Mathf.Lerp(startAlpha, 1, progress);
            m.SetColor(Outline, SetAlpha(c, alpha));
            yield return new WaitForEndOfFrame();
        }
        _fadingIn = false;
    }

    private static Color SetAlpha(Color c, float alpha) {
        return new Color(c.r, c.g, c.b, alpha);
    }

	void OnGUI() {
		if (CameraUtil.IsFaded)
			return;

		switch(SelectionMethod) { 
		case SelectionType.MIDDLE_SCREEN:
			GUIHelpers.DrawCrossHair(SelectionType.MIDDLE_SCREEN);
			break;
		case SelectionType.MOUSE:
			GUIHelpers.DrawCrossHair(SelectionType.MOUSE);
			break;
		}
	}
}