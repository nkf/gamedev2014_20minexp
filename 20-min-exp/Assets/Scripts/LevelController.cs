using System.Runtime.Serialization.Formatters;
using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	private int _currentLevelIndex;
    private bool _loading = false;

	/// <summary>
	/// Loads the next level in the game.
	/// </summary>
	public void LoadNext(float fadeTime = 3.0f) {
		Load (++_currentLevelIndex, fadeTime);
	}

	public void ReloadCurrent(float fadeTime = 3.0f) {
		Load (_currentLevelIndex, fadeTime);
	}

	protected void Load(int levelIndex, float fadeTime = 3.0f) {
		if (_loading)
			return;
		
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
