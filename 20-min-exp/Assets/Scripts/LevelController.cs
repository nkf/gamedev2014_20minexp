using System.Runtime.Serialization.Formatters;
using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {


    private int _currentLevelIndex;
    private bool _loading = false;

    /// <summary>
    /// Loads the next level in the game. This usually fucks up during testing, when the first scene isn't the table scene.
    /// </summary>
    public void LoadNext(float fadeTime = 3.0f) {
        Load(_currentLevelIndex + 1, fadeTime);
        // We don't want to increment _currentLevelIndex here, as it is set in Load()
    }

    public void ReloadCurrent(float fadeTime = 3.0f) {
        Load(_currentLevelIndex, fadeTime);
    }

    public void Load(int levelIndex, float fadeTime = 3.0f) {
        if (_loading)
            return;

        Debug.Log("Loading level: " + _currentLevelIndex);

        _currentLevelIndex = levelIndex;

        _loading = true;
        LevelLoader.Load(levelIndex);
        var fader = CameraUtil.GetFader();
        StartCoroutine(fader.FadeToBlack(fadeTime, () => {
            StartCoroutine(TrySwitch(fader));
            _loading = false;
        }));
    }

    private IEnumerator TrySwitch(Fader fader) {
        while (LevelLoader.Status == LoadStatus.Loading)
            yield return new WaitForEndOfFrame();
        if (LevelLoader.Status == LoadStatus.Done) {
            LevelLoader.Switch();
            var text = LevelText[_currentLevelIndex][Toolbox.Instance.gameState._dayCounter];
            StartCoroutine(Camera.main.ShowCenterText(text, () =>
                StartCoroutine(fader.FadeInFromBlack(3.0f, () => { }))
                ));
        }
    }

    // Scene indexes to increase readability
    public static readonly int TABLE = 0;
    public static readonly int HALL = 1;
    public static readonly int ROAD = 2;
    public static readonly int PARKING = 3;
    public static readonly int OFFICE = 4;
    public static readonly int WINDOW_SHOPPER = 5;
    public static readonly int APPEARANCES = 6;
    public static readonly int SELL_STUFF = 7;

    
    public static string[] TableText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] HallText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] RoadText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] ParkingText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] OfficeText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] ShopperText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] AppText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] SellText = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[][] LevelText = { TableText, HallText, RoadText, ParkingText, OfficeText, ShopperText, AppText, SellText };


    static LevelController() {
            TableText   [GameState.REGULAR_DAY]         = "FAMILY BREAKFAST";
            TableText   [GameState.FIRING_DAY_MORNING]  = "ANOTHER MORNING";
            TableText   [GameState.FIRING_DAY_AFTERNOON]= "THE SAME EVENING";
            TableText   [GameState.APPEARANCES_DAY_2]   = "";
            TableText   [GameState.APPEARANCES_DAY_3]   = "";
            HallText    [GameState.REGULAR_DAY]         = "GET TO THE CAR, GET TO WORK";
            HallText    [GameState.FIRING_DAY_MORNING]  = "I REPEAT THE SAME ROUTINE";
            RoadText    [GameState.REGULAR_DAY]         = "JUST GET TO WORK";
            RoadText    [GameState.FIRING_DAY_MORNING]  = "I THINK I USED TO HAVE A PURPOSE";
            ParkingText [GameState.REGULAR_DAY]         = "PARK THE CAR STEN, PARK THE CAR.";
            ParkingText [GameState.FIRING_DAY_MORNING]  = "THAT MIGHT HAVE BEEN A DREAM";
            OfficeText  [GameState.REGULAR_DAY]         = "WORK WORK?";
            OfficeText  [GameState.REGULAR_DAY]         = "WORK WORK?";
            ShopperText [GameState.FIRING_DAY_MORNING]  = "MIGHT ASWELL USE SOME MONEY";
            AppText     [GameState.APPEARANCES_DAY_1]   = "THEY CAN'T KNOW THAT IM NO LONGER WORKING";
            AppText     [GameState.APPEARANCES_DAY_2]   = "A FEW WEEKS LATER";
            AppText     [GameState.APPEARANCES_DAY_3]   = "LATER THAT MONTH";
            SellText    [GameState.SELL_STUFF_DAY]      = "I CAN TELL YOU EXACTLY HOW IT WILL END";
    }
}
