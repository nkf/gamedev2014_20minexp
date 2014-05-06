using System.Net.NetworkInformation;
using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {

    private TextMesh _textMesh;
	protected GameObject _bubble;
    public float Size;

	public string Text { get { return _textMesh.text; } }

	void Start () {
	    _textMesh = GetComponentInChildren<TextMesh>();
		foreach (Transform child in transform)
			if (child.name.Equals ("Bubble"))
			    _bubble = child.gameObject;

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
 
		int noOfLines = 1;
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
				noOfLines++;
            } else {
                newString = newString + word + " ";
            }
        }

		// Adjust height
		// NOTE: Totally hacky, ugly, not precise solution, though
		if (_bubble != null) {
			float padding = 0.15f;
			Vector3 newScale = new Vector3(transform.localScale.x,
			                               ((_textMesh.fontSize * (_textMesh.transform.localScale.y/10))*noOfLines)+padding,
			                               transform.localScale.z);

			_bubble.transform.localScale = newScale;
		}

        return newString;
    }
}
