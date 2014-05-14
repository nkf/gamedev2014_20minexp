using UnityEngine;
using System.Collections;

public class GetName : MonoBehaviour {

    private TextMesh _tag;
	void Start () {
	    _tag = GetComponent<TextMesh>();
	    _tag.text = Toolbox.Instance.gameState.CharacterName;
	}
}
