using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	public static readonly int REGULAR_DAY          = 0;
	public static readonly int FIRING_DAY_MORNING   = 1;
	public static readonly int FIRING_DAY_AFTERNOON = 2;
	public static readonly int APPEARANCES_DAY_1    = 3;
	public static readonly int APPEARANCES_DAY_2    = 4;
	public static readonly int APPEARANCES_DAY_3    = 5;
	public static readonly int SELL_STUFF_DAY       = 6;

    public static readonly int TOTAL_DAY_COUNTER    = 7;
    
	public int MoneyCounter { get; set; }

	public int _dayCounter = FIRING_DAY_MORNING;
	public int DayCounter   { get {return _dayCounter;} set {_dayCounter = value;} }

    // Use this for initialization
	void Start () {
	    MoneyCounter = 500000;
	    StartCoroutine(MoneyDecay());
	}

    IEnumerator MoneyDecay() {
        while (true) {
            yield return new WaitForSeconds(0.8f);
            MoneyCounter-=100;
        }
    }

	// Update is called once per frame
	void Update () {

	}

    private bool _showingCenterText;
    private string _centerText;
    private readonly Rect _center = new Rect(Screen.width/2f, Screen.height/2f, 0, 0);
    public void ShowCenterText(string text) {
        _showingCenterText = true;
        _centerText = text;
    }

    public void HideCenterText() {
        _showingCenterText = false;
    }

	void OnGUI() {
		// Render monies
		Vector2 textDimensions = GUI.skin.label.CalcSize( new GUIContent(Toolbox.Instance.gameState.MoneyCounter.ToString()) );
		Rect moneyGUI = new Rect(0, 0, Screen.width, 20);
		//Rect moneyGUI = new Rect(0, 0, 100, 100);
		GUIHelpers.DrawQuad(moneyGUI, Color.black);
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 20;
		style.alignment = TextAnchor.UpperCenter;
		style.normal.textColor = Color.white;
		GUI.Label(moneyGUI, "Bank Balance: $ " + Toolbox.Instance.gameState.MoneyCounter.ToString(), style);

	    if (_showingCenterText) {
	        GUI.Label(_center, _centerText, new GUIStyle {fontSize = 30, alignment = TextAnchor.MiddleCenter, normal = {textColor = Color.white}});
	    }
	}
}
