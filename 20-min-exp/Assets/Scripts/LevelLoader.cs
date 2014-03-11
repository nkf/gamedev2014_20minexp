using System.ComponentModel;
using UnityEngine;
using System.Collections;
using AsyncOperation = UnityEngine.AsyncOperation;

public class LevelLoader {

    private static AsyncOperation _currentLoadJob;
    /// <summary>
    /// Returns the current progess or -1 if there is no level loading.
    /// </summary>
    public static float Progress {
        get { return _currentLoadJob == null ? -1 : Mathf.Round(_currentLoadJob.progress * 100f) ; }
    }

    public static LoadStatus Status {
        get {
            return _currentLoadJob == null
                ? LoadStatus.NotLoading
                : _currentLoadJob.progress >= 0.9f ? LoadStatus.Done : LoadStatus.Loading; 
                //soo unity is fucked, loading will only go to 90% when allowSceneActivation is set to false and isDone will not trigger.
        }
    }

    public static bool Load(string level) {
        if (_currentLoadJob != null) {Debug.Log("Could not load \""+level+"\", because a level is already loading"); return false;}
        _currentLoadJob = Application.LoadLevelAsync(level);
        if(_currentLoadJob != null) _currentLoadJob.allowSceneActivation = false;
        return _currentLoadJob != null;
    }

    public static void Switch() {
        if (_currentLoadJob != null) {
            _currentLoadJob.allowSceneActivation = true;
            _currentLoadJob = null;
        } else {
            Debug.Log("Could not switch because no scene was loaded.");
        }
        
    }
}

public enum LoadStatus {
    Done, Loading, NotLoading
}