using System;
using UnityEngine;
using System.Collections;

public static class CameraUtil {

    public static IEnumerator FadeToBlack(this Camera camera, float time, Action onComplete) {
        var blackTexture = GameObject.Instantiate(Resources.Load<GameObject>("BlackScreen")) as GameObject;
        blackTexture.transform.position = new Vector3(0.5f, 0.5f);
        return FadeTo(blackTexture.GetComponent<GUITexture>(), time, onComplete);
    }

    private static IEnumerator Fader(GUITexture toFade, float time, Action onComplete, Func<float, float, float, float> alphaCalculator) {
        var c = toFade.color;
        var start = Time.time;
        var end = start + time;
        while (Time.time <= end) {
            var alpha = alphaCalculator(start, end, Time.time);
            var newColor = new Color(c.r, c.g, c.b, alpha);
            toFade.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        if (onComplete != null) onComplete();
    }

    private static IEnumerator FadeTo(GUITexture toFade, float time, Action onComplete) {
        return Fader(toFade, time, onComplete, Mathf.InverseLerp);
    }

    private static IEnumerator FadeFrom(GUITexture toFade, float time, Action onComplete) {
        return Fader(toFade, time, onComplete, (start, end, ttime) => Mathf.InverseLerp(end, start, ttime));
    }
}
