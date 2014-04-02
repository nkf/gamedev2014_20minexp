using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public int MoneyCounter { get; set; }

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
