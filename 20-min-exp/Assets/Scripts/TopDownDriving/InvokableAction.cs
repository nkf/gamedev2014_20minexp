using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public abstract class InvokableAction : MonoBehaviour {

	protected bool _actionAvailable = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.Equals(Car.PLAYER.gameObject) &&
		    !CameraUtil.IsFaded) {
			_actionAvailable = true;
		}
	}
	
	public virtual void OnTriggerExit(Collider collider) {
		if (collider.gameObject.Equals(Car.PLAYER.gameObject)) {
			_actionAvailable = false;
		}
	}

	//////////////////////////////////////
	/// LOL, FADING
	////////////////////////////////////
	
	protected const string Outline = "_OutlineColor";
	protected bool _fadingIn;
	protected readonly List<Material> _fadingOut = new List<Material>();
	protected Material _current;
	
	protected IEnumerator _FadeOut(Material m, float time) {
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
	
	protected IEnumerator _FadeIn(Material m, float time) {
		_fadingIn = true;
		var c = m.GetColor(Outline);
		var startAlpha = c.a;
		var start = Time.time;
		var end = start + time;
		while(Time.time <= end) {
			var progress = Mathf.InverseLerp(start, end, Time.time);
			var alpha = Mathf.Lerp(startAlpha, 1, progress);
			m.SetColor(Outline, SetAlpha(c, alpha));
			yield return new WaitForEndOfFrame();
		}
		m.SetColor(Outline, SetAlpha(c, 1));
		_fadingIn = false;
	}
	
	protected static Color SetAlpha(Color c, float alpha) {
		return new Color(c.r, c.g, c.b, alpha);
	}
}
