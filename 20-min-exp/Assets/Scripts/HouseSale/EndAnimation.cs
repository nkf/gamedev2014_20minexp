using UnityEngine;
using System.Collections;

public class EndAnimation : MonoBehaviour {

    public Animation AnimationToPlayAtLastSale;
    private static Animation _animation;
    void Awake() {
        _animation = AnimationToPlayAtLastSale;
    }
    public static void Play() {
        _animation.Play();
    }

}
