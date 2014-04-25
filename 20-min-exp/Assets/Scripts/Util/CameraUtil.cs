using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using Object = UnityEngine.Object;

public static class CameraUtil {

    public static Fader GetFader() {
        return new Fader();
    }

    private static IEnumerator Fader(GUITexture toFade, float time, Action onComplete, Func<float, float, float, float> alphaCalculator) {
        var c = toFade.color;
        var start = Time.time;
        var end = start + time;
        while (Time.time <= end) {
            var t = alphaCalculator(start, end, Time.time);
            var alpha = Mathf.Lerp(0, 0.75f, t);
            var newColor = new Color(c.r, c.g, c.b, alpha);
            toFade.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        if (onComplete != null) onComplete();
    }

    public static IEnumerator FadeTo(GUITexture toFade, float time, Action onComplete) {
        return Fader(toFade, time, onComplete, Mathf.InverseLerp);
    }

    public static IEnumerator FadeFrom(GUITexture toFade, float time, Action onComplete) {
        return Fader(toFade, time, onComplete, (start, end, ttime) => Mathf.InverseLerp(end, start, ttime)) ;
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
        yield return new WaitForSeconds(5);
        Toolbox.Instance.gameState.HideCenterText();
        onComplete();
    }
}
public class Fader {
    public GameObject _blackScreen;
    public IEnumerator FadeToBlack(float time, Action onComplete) {
        _blackScreen = GameObject.Instantiate(Resources.Load<GameObject>("BlackScreen")) as GameObject;
        _blackScreen.transform.position = new Vector3(0.5f, 0.5f);
        Object.DontDestroyOnLoad(_blackScreen);
        return CameraUtil.FadeTo(_blackScreen.GetComponent<GUITexture>(), time, onComplete);
    }

    public IEnumerator FadeInFromBlack(float time, Action onComplete) {
        return CameraUtil.FadeFrom(_blackScreen.GetComponent<GUITexture>(), time, () => {
            Object.Destroy(_blackScreen);
            onComplete();
        });
    }

    public void Clear() {
        Object.Destroy(_blackScreen);
    }
}
