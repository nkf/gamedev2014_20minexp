using UnityEngine;
using System.Collections;
[RequireComponent(typeof(GUIText))]
public class LevelController : MonoBehaviour {

    private GUIText _text;
	// Use this for initialization
	void Start () {
	    _text = GetComponent<GUIText>();
        _text.pixelOffset = new Vector2(Screen.width/2, Screen.height/2);
	    _text.enabled = false;
	}
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private int _index = 1;
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyUp(KeyCode.F7)) {
	        if (LevelLoader.Load("level" + _index)) {
                StartCoroutine(LoadingText());
	            _index--;
	        }
	    } else if (Input.GetKeyUp(KeyCode.F8)) {
	        if (LevelLoader.Load("level" + _index)) {
                StartCoroutine(LoadingText());
	            _index++;
	        }
	    } else if (Input.GetKeyUp(KeyCode.F9)) {
	        _text.enabled = false;
	        LevelLoader.Switch();
	    }
	}

    IEnumerator LoadingText() {
        if (LevelLoader.Status == LoadStatus.NotLoading) yield break;
        _text.enabled = true;
        while (LevelLoader.Status == LoadStatus.Loading) {
            _text.text = LevelLoader.Progress+"%";
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        _text.text = 100 + "%";
    }
}
