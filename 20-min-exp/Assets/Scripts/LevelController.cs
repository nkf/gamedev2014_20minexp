using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUIText))]
public class LevelController {

	private int _currentLevelIndex;
	private string[] _levelNames;
//    private GUIText _text;

	public LevelController() {
		_levelNames = new string[3];
		_levelNames[0] = "House";
		_levelNames[1] = "Road";
		_levelNames[2] = "Office";
	}

	/// <summary>
	/// Loads the next level in the game.
	/// </summary>
	public void LoadNext() {
		// TODO: Go to some fade-out crap
		LevelLoader.Load( _levelNames[_currentLevelIndex+1] );
		// TODO: Render load progress

		// When the scene has loaded, switch to it
		LevelLoader.Switch();
	}

    IEnumerator LoadingText() {
        if (LevelLoader.Status == LoadStatus.NotLoading) yield break;
//        _text.enabled = true;
        while (LevelLoader.Status == LoadStatus.Loading) {
//            _text.text = LevelLoader.Progress+"%";
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
//        _text.text = 100 + "%";
    }
}
