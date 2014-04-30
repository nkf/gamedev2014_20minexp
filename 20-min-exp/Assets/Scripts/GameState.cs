using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class GameState : MonoBehaviour {

	public static readonly int REGULAR_DAY          = 0;
	public static readonly int FIRING_DAY_MORNING   = 1;
	public static readonly int FIRING_DAY_AFTERNOON = 2;
	public static readonly int APPEARANCES_DAY_1    = 3;
	public static readonly int APPEARANCES_DAY_2    = 4;
	public static readonly int APPEARANCES_DAY_3    = 5;
	public static readonly int SELL_STUFF_DAY       = 6;

    public static readonly int TOTAL_DAY_COUNTER    = 7;

    private int _moneyCounter;
    private int _renderMoney;
    public int MoneyCounter {
        get {
            return _moneyCounter;
        }
        set {
            AddJuicyMoney(value - _moneyCounter);
            _moneyCounter = value;
        }
    }

    public int _dayCounter = REGULAR_DAY;
	public int DayCounter   { get {return _dayCounter;} set {_dayCounter = value;} }

    // Use this for initialization
	void Start () {
	    _moneyCounter = 48320;
	    _renderMoney = 0;
	    StartCoroutine(MoneyDecay());
	}

    IEnumerator MoneyDecay() {
        while (true) {
            yield return new WaitForSeconds(1f);
            _moneyCounter -= Random.Range(2,7);
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
        var diff = _moneyCounter - _renderMoney;
        if(Mathf.Abs(diff) > 0) {
            var a = Mathf.Max(1, diff/15f);
            _renderMoney += (int) (Mathf.Sign(diff)*a);
        }

		var moneyGUI = new Rect(0, 0, Screen.width, 20);		
		var style = new GUIStyle {fontSize = 20, alignment = TextAnchor.UpperCenter, normal = {textColor = Color.white}};
	    var moneyText = "Bank Balance: $ " + _renderMoney;
        var moneyTextDimensions = GUI.skin.label.CalcSize(new GUIContent(moneyText));
        
        //Draw blackbar + money
        GUIHelpers.DrawQuad(moneyGUI, Color.black);
        GUI.Label(moneyGUI, moneyText, style);
        //Draw money additions.
	    var moneyDrawOffset = Screen.width/2f + moneyTextDimensions.x; //the money text is centered in the middle of the screen.
	    foreach (var moneyAddition in _moneyAdditions) {
	        var positive = Mathf.Sign(moneyAddition.Amount) > 0;
	        var addText =  positive ? "+" + moneyAddition.Amount : "" + moneyAddition.Amount;
	        var addSize = GUI.skin.label.CalcSize(new GUIContent(addText));
	        var addStyle = new GUIStyle(style) {normal = {textColor = positive ? Color.green : Color.red}};
	        GUI.Label(new Rect(moneyDrawOffset,0, addSize.x, addSize.y),addText, addStyle);
	        moneyDrawOffset += addSize.x*2;
	    }
	    _moneyAdditions.RemoveAll(m => m.ExpirationTime < Time.time);

        //Draw centertext
	    if (_showingCenterText) {
	        GUI.Label(_center, _centerText, new GUIStyle {fontSize = 30, alignment = TextAnchor.MiddleCenter, normal = {textColor = Color.white}});
	    }
	}

    private struct MoneyAddition {
        public int Amount;
        public float ExpirationTime;
    }
    private readonly List<MoneyAddition> _moneyAdditions = new List<MoneyAddition>(); 
    public void AddJuicyMoney(int n) {
        _moneyAdditions.Add(new MoneyAddition {
            Amount = n, 
            ExpirationTime = Time.time+2f
        });
    }
    private string additionString() {
        return _moneyAdditions.Aggregate("[", (current, ma) => current + "("+ ma.Amount + "," + ma.ExpirationTime + ") ") + "]";
    }
}
