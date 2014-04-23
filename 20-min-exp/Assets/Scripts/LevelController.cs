using System.Runtime.Serialization.Formatters;
using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	// Scene indexes to increase readability
	public static readonly int TABLE    = 0;
	public static readonly int HALL     = 1;
	public static readonly int ROAD     = 2;
	public static readonly int PARKING  = 3;
	public static readonly int OFFICE   = 4;
	public static readonly int WINDOW_SHOPPER  = 5;
	public static readonly int APPEARANCES = 6;
	public static readonly int SELL_STUFF  = 7;

	private int _currentLevelIndex;
    private bool _loading = false;

	/// <summary>
	/// Loads the next level in the game. This usually fucks up during testing, when the first scene isn't the table scene.
	/// </summary>
	public void LoadNext(float fadeTime = 3.0f, int increaseDayCounter = 0) {
		Load (_currentLevelIndex + 1, fadeTime); // We don't want to increment _currentLevelIndex here, as it is set in Load()
	}

	public void ReloadCurrent(float fadeTime = 3.0f) {
		Load (_currentLevelIndex, fadeTime);
	}

	public void Load(int levelIndex, float fadeTime = 3.0f) {
		if (_loading)
			return;

		Debug.Log ("Loading level: "+_currentLevelIndex);

		_currentLevelIndex = levelIndex;
		
		_loading = true;
		LevelLoader.Load(levelIndex);
		StartCoroutine(Camera.main.FadeToBlack(fadeTime, () => {
			StartCoroutine(TrySwitch());
			_loading = false;
		}));
	}

    static IEnumerator TrySwitch() {
        while (LevelLoader.Status == LoadStatus.Loading)
			yield return new WaitForEndOfFrame();
        if (LevelLoader.Status == LoadStatus.Done)
			LevelLoader.Switch();
    }
}
