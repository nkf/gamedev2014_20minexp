using UnityEngine;
using System.Collections;

public class EndAnimation : MonoBehaviour {

    public Animation AnimationToPlayAtLastSale;
    private static Animation _animation;
    private static EndAnimation thisO;
    void Awake() {
        _animation = AnimationToPlayAtLastSale;
        thisO = this;
    }

    private void End() {
        StartCoroutine(CameraUtil.GetFader().FadeToBlack(10f, () => { Application.Quit(); }));
    }
    public static void Play() {
        thisO.End();
        //_animation.Play();
    }

}
