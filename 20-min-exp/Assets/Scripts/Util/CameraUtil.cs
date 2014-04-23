using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public static class CameraUtil {

	public static GameObject _blackScreen;

    public static IEnumerator FadeToBlack(this Camera camera, float time, Action onComplete) {
        _blackScreen = GameObject.Instantiate(Resources.Load<GameObject>("BlackScreen")) as GameObject;
        _blackScreen.transform.position = new Vector3(0.5f, 0.5f);
        Object.DontDestroyOnLoad(_blackScreen);
        return FadeTo(_blackScreen.GetComponent<GUITexture>(), time, onComplete);
    }

	public static IEnumerator FadeInFromBlack(this Camera camera, float time, Action onComplete) {
		return FadeFrom(_blackScreen.GetComponent<GUITexture>(), time, onComplete);
	}

    private static IEnumerator Fader(GUITexture toFade, float time, Action onComplete, Func<float, float, float, float> alphaCalculator, bool destroyBlackScreen) {
        var c = toFade.color;
        var start = Time.time;
        var end = start + time/2; //yeah for some reason we only need half the alpha?
        while (Time.time <= end) {
            var alpha = alphaCalculator(start, end, Time.time);
            var newColor = new Color(c.r, c.g, c.b, alpha);
            toFade.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        if (onComplete != null) onComplete();
		if (destroyBlackScreen) UnityEngine.Object.Destroy(_blackScreen);
    }

    private static IEnumerator FadeTo(GUITexture toFade, float time, Action onComplete) {
        return Fader(toFade, time, onComplete, Mathf.InverseLerp, false);
    }

    private static IEnumerator FadeFrom(GUITexture toFade, float time, Action onComplete) {
        return Fader(toFade, time, onComplete, (start, end, ttime) => Mathf.InverseLerp(end, start, ttime), true) ;
    }
    
    public static void SwitchTo(this Camera c) {
        AudioListener al;
        foreach(var camera in Camera.allCameras.Where(camera => camera != c)) {
            camera.enabled = false;
            al = camera.gameObject.GetComponent<AudioListener>();
            if(al != null) al.enabled = false;
        }
        c.enabled = true;
        al = c.gameObject.GetComponent<AudioListener>();
        if(al != null) al.enabled = true;
    }

    public static IEnumerator ShowCenterText(this Camera camera, string text, Action onComplete) {
        Toolbox.Instance.gameState.ShowCenterText(text);
        yield return new WaitForSeconds(3);
        Toolbox.Instance.gameState.HideCenterText();
        onComplete();
    }
}
