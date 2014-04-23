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
    
	public int MoneyCounter { get; set; }

	public int _dayCounter = REGULAR_DAY;
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

	void OnGUI() {
		// Render monies
		Vector2 textDimensions = GUI.skin.label.CalcSize( new GUIContent(Toolbox.Instance.gameState.MoneyCounter.ToString()) );
		Rect moneyGUI = new Rect(0, 0, textDimensions.x+10, 20);
		GUIHelpers.DrawQuad(moneyGUI, Color.black);
		
		GUIStyle style = new GUIStyle();
		style.fontSize = 15;
		style.alignment = TextAnchor.UpperCenter;
		style.normal.textColor = Color.white;
		GUI.Label(moneyGUI, Toolbox.Instance.gameState.MoneyCounter.ToString(), style);
	}
}
