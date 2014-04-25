using UnityEngine;
using System;
using System.IO;
using System.Collections;
using SimpleJSON;

/// <summary>
/// Conversation agents should extend this class in order to specify how the conversation should work.
/// For instance, when to start it and if there are any conditions, that should end the conversation
/// prematurely.
/// </summary>
public class ConversationAgent : MonoBehaviour {

	protected enum ConversationState { AWKWARD, SELECTION_TIME, ANSWERING_APPERANCE_DELAY, DIALOGUE_PAUSE };
	
	public string _conversationPath;	// Relative path to the JSON conversation file

	protected int currentNode = 0;		// Index of the current nodes
	protected ConversationNode[] allNodes;// Collection of all the nodes

	protected bool _conversationIsRunning = false;
	protected int _highlightedResponse = 0;

	/// <summary>
	/// The conversation state should loop between DIALOGUE_PAUSE, ANSWERING_APPEARANCE_DELAY, SELECTION_TIME, AWKWARD starting with DIALOGUE_PAUSE
	/// to ensure that next node delays on the first node is handled correctly.
	/// </summary>
	protected ConversationState _state = ConversationState.DIALOGUE_PAUSE;

	private float endTimer;				// Used to time the display time of the last conversation dialogue
	private float nodeTimer;
	private bool nodeTimerHasBeenSet = false;

	public bool IsRunning { get {return _conversationIsRunning;} }

	// Use this for initialization
	protected virtual void Start () {

	}

	bool endTimerHasBeenSet = false;
	// Update is called once per frame
	protected virtual void Update () {
		if (!_conversationIsRunning)
			return;

		// Check whether conversation should skip ahead if no answer is selected
		if (getCurrentNode().SilentResponse > 0.0) {
			if (!nodeTimerHasBeenSet) {
				nodeTimer = Time.time;
				nodeTimerHasBeenSet = true;
			}

			if (Time.time - nodeTimer > getCurrentNode().SilentResponse) {
				currentNode = getCurrentNode().SilentGoto;
				nodeTimerHasBeenSet = false;
			}
		}

		switch(_state) {
		case ConversationState.SELECTION_TIME:
			// Controls
			if (Input.GetKey(KeyCode.UpArrow) && _highlightedResponse != 0) {
				_highlightedResponse--;
			}
			if (Input.GetKey (KeyCode.DownArrow) && _highlightedResponse != getCurrentNode().Responses.Length-1) {
				_highlightedResponse++;
			}
			if (Input.GetKeyDown (KeyCode.Return) && !getCurrentNode().IsEndNode) {
				//_highlightedResponse = 0;
				_state = ConversationState.AWKWARD;
				endTimer = Time.time;
				if (getCurrentNode().IsEndNode)
					endTimer = Time.time;
			}

			int endConvoNodeDisplayTime = 3;
			if (getCurrentNode().IsEndNode &&
			    (Time.time - endTimer) > endConvoNodeDisplayTime)
				StopConversation();

			break;

		case ConversationState.AWKWARD:
			if (Time.time - endTimer > getCurrentNode().AnsweringDelay) {
				endTimer = Time.time;
				_state = ConversationState.DIALOGUE_PAUSE;
				SelectResponse(_highlightedResponse);
			}
			break;
		case ConversationState.ANSWERING_APPERANCE_DELAY:
			if (!endTimerHasBeenSet) {
				endTimer = Time.time;
				endTimerHasBeenSet = true;
			}

			if ((Time.time - endTimer) > getCurrentNode().AnswerAppearanceDelay) {
				_state = ConversationState.SELECTION_TIME;
				endTimerHasBeenSet = false;
			}
			break;
		case ConversationState.DIALOGUE_PAUSE:
			if (!endTimerHasBeenSet) {
				endTimer = Time.time;
				endTimerHasBeenSet = true;
			}

			if ((Time.time - endTimer) > getCurrentNode().NextNodeDelay) {
				_state = ConversationState.ANSWERING_APPERANCE_DELAY;
				endTimerHasBeenSet = false;
			}
			break;
		}
	}

	void OnGUI() {
		if (!_conversationIsRunning)
			return;

		int barHeight = (Screen.height/5);
		Rect posBackground  = new Rect(0, Screen.height-barHeight, Screen.width, barHeight);

		// Draw Background
		Texture2D texture = new Texture2D(1, 1);
		Color color = new Color();
		color = Color.black;
		texture.SetPixel(0,0, color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(posBackground, GUIContent.none);


		switch(_state) {
		case ConversationState.SELECTION_TIME:
			RenderDialogue(posBackground);
			// Write responses
			string[] responses = getCurrentNode().Responses;
			for(int i = 0; i < responses.Length; i++) {
				RenderAnswer(i, barHeight);
			}
			break;

		case ConversationState.AWKWARD:
			RenderDialogue(posBackground);
			RenderAnswer(_highlightedResponse, barHeight);
			break;
		case ConversationState.ANSWERING_APPERANCE_DELAY:
			RenderDialogue(posBackground);
			// Don't render responses
			break;
		case ConversationState.DIALOGUE_PAUSE:
			// Don't anything but the conversation background
			break;
		}
	}

	protected void RenderDialogue(Rect position) {
		// Write Dialogue
		GUIStyle style = new GUIStyle();
		style.fontSize = 15;
		style.alignment = TextAnchor.MiddleCenter;
		style.normal.textColor = Color.white;
		GUI.Label(position, getCurrentNode().Dialogue, style);
	}

	protected void RenderAnswer(int i, int height) {
		int dialogueOffset = 15; // Offsets the responses in the y-direction so they don't clash with the displayed dialogue.

		Rect responsePos = new Rect(0, Screen.height-height+(i*15)+dialogueOffset, Screen.width, 15);
	
		if (i == _highlightedResponse) {
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0,0, Color.white);
			texture.Apply();
			GUI.skin.box.normal.background = texture;
			GUI.Box(responsePos, GUIContent.none);
		}
	
		GUIStyle style = new GUIStyle();
		style.fontSize = 15;
		style.alignment = TextAnchor.UpperCenter;
		if (i == _highlightedResponse) {
			style.normal.textColor = Color.black;
		} else {
			style.normal.textColor = Color.white;
		}
		GUI.Label(responsePos, "[ " + getCurrentNode().Responses[i] + " ]", style);
	}

	public void StartConversation() {
		// First, load conversation resource
		// Modifies the path to use the path seperator for the current system
		_conversationPath = _conversationPath.Replace(@"/", Path.DirectorySeparatorChar.ToString());
		_conversationPath = _conversationPath.Replace(@"\", Path.DirectorySeparatorChar.ToString());	

		TextAsset txt = (TextAsset)Resources.Load(_conversationPath, typeof(TextAsset));

		// Read in the conversation from the JSON file and initialise the node collection
		JSONNode node = JSONNode.Parse(txt.text);
		allNodes = new ConversationNode[ node["AllNodes"].Count ];
		
		
		// Build all ConversationNodes and add to allNodes
		foreach(JSONNode n in node["AllNodes"].Childs) {
			// If answeringDelay is undefined, it will default to 0.0
			ConversationNode cNode = new ConversationNode(n["dialogue"], n["responses"].Count, n["answeringDelay"].AsFloat);
			
			// Set up responses
			int i = 0; // .Childs doesn't have Count/Size field
			foreach(JSONNode response in n["responses"].Childs) {
				cNode.Responses[i] = response["response"];
				cNode.NodeLinks[i] = response["gotoNode"].AsInt;
				i++;
			}
			
			var silResp = n["silentResponse"];
			if (silResp != null) {
				cNode.SilentResponse = silResp["timer"].AsFloat;
				cNode.SilentGoto     = silResp["gotoNode"].AsInt;
			}
			
			JSONNode answerAppearanceDelay = n["answeringAppearanceDelay"];
			cNode.AnswerAppearanceDelay = answerAppearanceDelay.AsFloat;
			
			JSONNode nextNodeDelay = n["nextNodeDelay"];
			cNode.NextNodeDelay = nextNodeDelay.AsFloat;
			
			// Add to allNodes
			allNodes[ n["index"].AsInt ] = cNode;
		}

		// Then set the conversation as running
		_conversationIsRunning = true;
		//SimplePlayer.PLAYER.SwitchState(PlayerState.IN_CONVERSATION);
	}

	protected virtual void StopConversation() {
		_conversationIsRunning = false;
		//SimplePlayer.PLAYER.SwitchState(PlayerState.PLAYING);
		Restart();
	}

	public ConversationNode getCurrentNode() {
		return allNodes[currentNode];
	}

	public void Restart() {
		currentNode = 0;
	}
	
	public void SelectResponse(int responseIndex) {
		currentNode = getCurrentNode().NodeLinks[responseIndex];
		_highlightedResponse = 0;
	}
}
