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

    IEnumerator TrySwitch() {
        while (LevelLoader.Status == LoadStatus.Loading)
			yield return new WaitForEndOfFrame();
        if (LevelLoader.Status == LoadStatus.Done) {
            LevelLoader.Switch();
            var text = LevelText[_currentLevelIndex][Toolbox.Instance.gameState._dayCounter];
            StartCoroutine(Camera.main.ShowCenterText(text,() => 
                StartCoroutine(Camera.main.FadeInFromBlack(3.0f, () => { }))
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

    public static string[][] LevelText  = new string[8][];
    public static string[] TabelText    = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] HallText     = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] RoadText     = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] ParkingText  = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] OfficeText   = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] ShopperText  = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] AppText      = new string[GameState.TOTAL_DAY_COUNTER];
    public static string[] SellText     = new string[GameState.TOTAL_DAY_COUNTER];

    static LevelController() {
        TabelText   [GameState.REGULAR_DAY]         = "YOU ARE AT TABLE DAY 1";
        TabelText   [GameState.FIRING_DAY_MORNING]  = "YOU ARE AT TABLE DAY 2";
        TabelText   [GameState.FIRING_DAY_AFTERNOON]= "YOU ARE AT TABLE DAY 2 EVENING";
        TabelText   [GameState.APPEARANCES_DAY_2]   = "YOU ARE AT TABLE DAY 4";
        TabelText   [GameState.APPEARANCES_DAY_3]   = "YOU ARE AT TABLE DAY 5";
        HallText    [GameState.REGULAR_DAY]         = "YOU WALK THE HALL DAY 1";
        HallText    [GameState.FIRING_DAY_MORNING]  = "YOU WALK THE HALL DAY 2";
        RoadText    [GameState.REGULAR_DAY]         = "YOU ARE ON THE ROAD DAY 1";
        RoadText    [GameState.FIRING_DAY_MORNING]  = "YOU ARE ON THE ROAD DAY 2";
        ParkingText [GameState.REGULAR_DAY]         = "PARK THE CAR NIGGA DAY 1";
        ParkingText [GameState.FIRING_DAY_MORNING]  = "PARK THE CAR NIGGA DAY 2 (SPOILER: YOU ARE FIRED)";
        OfficeText  [GameState.REGULAR_DAY]         = "HAPPY DAY AT THE OFFICE DAY 1";
        OfficeText  [GameState.REGULAR_DAY]         = "SHIT DAY AT THE OFFICE DAY 2";
        ShopperText [GameState.FIRING_DAY_MORNING]  = "SHOP FOR THE BITCHES @ HOME DAY 2";
        AppText     [GameState.APPEARANCES_DAY_1]   = "YOU GOTTA PRETEND NOW NIGGA DAY 3";
        AppText     [GameState.APPEARANCES_DAY_2]   = "YOU GOTTA PRETEND NOW NIGGA DAY 4";
        AppText     [GameState.APPEARANCES_DAY_3]   = "YOU GOTTA PRETEND NOW NIGGA DAY 5";
        SellText    [GameState.SELL_STUFF_DAY]      = "SELL TILL YO RICH DAY 7";

        LevelText[0] = TabelText;
        LevelText[1] = HallText;
        LevelText[2] = RoadText;
        LevelText[3] = ParkingText;
        LevelText[4] = OfficeText;
        LevelText[5] = ShopperText;
        LevelText[6] = AppText;
        LevelText[7] = SellText;
    }
}
