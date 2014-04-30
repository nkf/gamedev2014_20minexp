using System.Net.NetworkInformation;
using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {

    private TextMesh _textMesh;
    public float Size;
	void Start () {
	    _textMesh = GetComponentInChildren<TextMesh>();
        SetText(_textMesh.text);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void SetText(string text) {
        var formattedText = formatText(text, _textMesh, Size);
        _textMesh.text = formattedText;
    }

    private string formatText( string textToFormat, TextMesh textObj, float desiredWidthOfMesh) {
        var words = textToFormat.Split(" "[0]);
        var newString = "";
        var testString = "";
 
        foreach (var word in words) {
            testString = testString + word + " ";
            textObj.text = testString;
 
            var textRot = textObj.transform.rotation;
            textObj.transform.rotation = Quaternion.identity;
            var textSize = textObj.renderer.bounds.size.x;
            textObj.transform.rotation = textRot;
            //Debug.Log(textSize);
            if (textSize > desiredWidthOfMesh) {
                testString = word + " ";
                newString = newString + "\n" + word + " ";
            } else {
                newString = newString + word + " ";
            }
        }
        return newString;
    }
}
