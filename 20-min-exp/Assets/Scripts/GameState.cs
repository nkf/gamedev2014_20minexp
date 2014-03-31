using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {

	private int _moneyCounter;
	public int MoneyCounter {
		get { return _moneyCounter; }
		set { _moneyCounter = value; }
	}

	// Use this for initialization
	void Start () {
		MoneyCounter = 34000;
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
