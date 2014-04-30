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

	protected enum ConversationState { DIALOGUE_PAUSE, RENDERING_DIALOGUE, ANSWERING_APPERANCE_DELAY, SELECTION_TIME, AWKWARD };
	
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
	public ConversationNode CurrentNode { get { return allNodes[currentNode]; } }

	// Use this for initialization
	protected virtual void Start () {
		lastTick = Time.time;
	}

	bool endTimerHasBeenSet = false;
	// Update is called once per frame
	protected virtual void Update () {
		if (!_conversationIsRunning)
			return;

		switch(_state) {
		case ConversationState.DIALOGUE_PAUSE:
			// Pause between two conversation nodes
			if (!endTimerHasBeenSet) {
				endTimer = Time.time;
				endTimerHasBeenSet = true;
			}
			
			if ((Time.time - endTimer) > CurrentNode.NextNodeDelay) {
				_state = ConversationState.RENDERING_DIALOGUE;
				endTimerHasBeenSet = false;
				lastTick = Time.time;
			}
			break;
		case ConversationState.RENDERING_DIALOGUE:
			int derp = (int) ((Time.time-lastTick) * charsPerSec);

			if (derp > CurrentNode.Dialogue.Length || // The dialogue has finished rendering, or...
			    Input.GetKeyDown(KeyCode.Return))     // ...The player wants to skip the JRPG thang?
			{
				_state = ConversationState.ANSWERING_APPERANCE_DELAY;
				lastTick = -1000; // minus a lot, because if lastTick = 0 the time difference may not be big enough to cause the entire string to be rendered.
			}
			break;
		case ConversationState.ANSWERING_APPERANCE_DELAY:
			// Wait before showing the responses and start listening for player input
			if (!endTimerHasBeenSet) {
				endTimer = Time.time;
				endTimerHasBeenSet = true;
			}



			if ((Time.time - endTimer) > CurrentNode.AnswerAppearanceDelay) {
				_state = ConversationState.SELECTION_TIME;
				endTimerHasBeenSet = false;
			}
			break;
		case ConversationState.SELECTION_TIME:
			// Controls
			// This state will wait for player input
			if ( (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) &&
			    _highlightedResponse != 0) {
				_highlightedResponse--;
			}
			if ( (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) &&
			    _highlightedResponse != CurrentNode.Responses.Length-1) {
				_highlightedResponse++;
			}
			if (Input.GetKeyDown (KeyCode.Return) && !CurrentNode.IsEndNode) {
				// When player selects a response, transition to the AWKWARD state.
				_state = ConversationState.AWKWARD;
				lastTick = Time.time;
				if (CurrentNode.IsEndNode)
					endTimer = Time.time;
			}

			// Check whether conversation should skip ahead if no answer is selected
			if (CurrentNode.SilentResponse > 0.0) {
				if (!nodeTimerHasBeenSet) {
					nodeTimer = Time.time;
					nodeTimerHasBeenSet = true;
				}
				
				if (Time.time - nodeTimer > CurrentNode.SilentResponse) {
					nodeTimerHasBeenSet = false;
					GoToNode(CurrentNode.SilentGoto);
				}
			}

			int endConvoNodeDisplayTime = 3;
			if (CurrentNode.IsEndNode && CurrentNode.SilentResponse <= 0.0 &&
			    (Time.time - endTimer) > endConvoNodeDisplayTime)
				StopConversation();

			break;
		case ConversationState.AWKWARD:
			// The AWKWARD state will wait awkwardly after the player selects a response before actually selecting the response and moving to next node
			// When the waiting time is 0, the response will be selected instantly. That is why SELECTION_TIME doesn't select responses directly.
			if (Time.time - endTimer > CurrentNode.AnsweringDelay) {
				endTimer = Time.time;
				_state = ConversationState.DIALOGUE_PAUSE;
				lastTick = Time.time;
				SelectResponse(_highlightedResponse);
			}
			break;
		
		}
	}


	protected int dialogueFontSize = 15;
	void OnGUI() {
		if (!_conversationIsRunning)
			return;

		int barHeight = (Screen.height/5);
		Rect posBackground  = new Rect(0, Screen.height-barHeight, Screen.width, barHeight);
		Rect posDialogue = new Rect(posBackground.x, posBackground.y, posBackground.width, posBackground.height);
		posDialogue.y += posDialogue.height/10; 
		// Draw Background
		Color color = Color.black;
		GUIHelpers.DrawQuad(posBackground, color);


		switch(_state) {
		case ConversationState.DIALOGUE_PAUSE:
			// Don't anything but the conversation background
			break;
		case ConversationState.RENDERING_DIALOGUE:
			RenderDialogue(posDialogue);
			break;
		case ConversationState.ANSWERING_APPERANCE_DELAY:
			RenderDialogue(posDialogue);
			// Don't render responses
			break;
		case ConversationState.AWKWARD:
			RenderDialogue(posDialogue);
			RenderAnswer(_highlightedResponse, barHeight, posDialogue.y+posDialogue.height);
			break;
		case ConversationState.SELECTION_TIME:
			RenderDialogue(posDialogue);
			// Write responses
			string[] responses = CurrentNode.Responses;
			for(int i = 0; i < responses.Length; i++) {
				RenderAnswer(i, barHeight, posDialogue.y+15);
			}
			break;
		}
	}

	protected float lastTick;
	public int charsPerSec = 105;
	protected void RenderDialogue(Rect position) {
		int derp = (int) ((Time.time-lastTick) * charsPerSec);

		// Write Dialogue
		GUIStyle style = new GUIStyle();
		style.fontSize = dialogueFontSize;
		style.alignment = TextAnchor.UpperCenter;
		style.normal.textColor = Color.white;
		if (derp <= CurrentNode.Dialogue.Length)
			GUI.Label(position, CurrentNode.Dialogue.Substring(0, derp), style);
		else
			GUI.Label(position, CurrentNode.Dialogue, style);
	}

	protected void RenderAnswer(int i, int height, float dialogueOffset) {
		int dialogueOffsetInt = (int) dialogueOffset; // Offsets the responses in the y-direction so they don't clash with the displayed dialogue.
		Rect responsePos = new Rect(0, (i*dialogueFontSize)+dialogueOffsetInt+5, Screen.width, dialogueFontSize);
	
		if (i == _highlightedResponse) {
			GUIHelpers.DrawQuad(responsePos, Color.white);
		}
	
		GUIStyle style = new GUIStyle();
		style.fontSize = dialogueFontSize;
		style.alignment = TextAnchor.UpperCenter;
		if (i == _highlightedResponse) {
			style.normal.textColor = Color.black;
		} else {
			style.normal.textColor = Color.white;
		}
		GUI.Label(responsePos, "[ " + CurrentNode.Responses[i] + " ]", style);
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
		lastTick = Time.time;
		//SimplePlayer.PLAYER.SwitchState(PlayerState.IN_CONVERSATION);
	}

	protected virtual void StopConversation() {
		_conversationIsRunning = false;
		//SimplePlayer.PLAYER.SwitchState(PlayerState.PLAYING);
		Restart();
	}

	public void Restart() {
		currentNode = 0;
	}
	
	public void SelectResponse(int responseIndex) {
		GoToNode (CurrentNode.NodeLinks[responseIndex]);
		_highlightedResponse = 0;
	}

	protected void GoToNode(int index) {
		currentNode = index;
		lastTick = Time.time;
		_state = ConversationState.DIALOGUE_PAUSE;
	}
}
