using UnityEngine;
using System.Collections;
using SimpleJSON;

public class ConversationNode {

	protected string   _dialogue;
	protected string[] _responses;
	protected float    _answeringDelay; // Delay after choosing a response
	// The indexes in this array points to indexes in an array which contains all conversation nodes.
	protected int[]    _nodeLinks;
	protected float    _silentResponse;
	protected int      _silentGoto;

	public string Dialogue      { get { return _dialogue; } }
	public string[] Responses   { get { return _responses; } }
	public float AnsweringDelay { get { return _answeringDelay; } }
	public int[] NodeLinks      { get { return _nodeLinks; } }
	public float SilentResponse {
		set { _silentResponse = value; }
		get { return _silentResponse; }
	}
	public int SilentGoto       {
		set { _silentGoto = value; }
		get { return _silentGoto; }
	}

	/**
	 * Whether this node is an end node. This is currently true, if there are no responses available.
	 * This can be changed, if we want dialogues to be able to be "two-fold". I.e. the conversation partner
	 * is able have consectuive nodes (which would progress over time) without the need for a response from the player.
	 */
	public bool IsEndNode {
		get {
			return _nodeLinks.Length == 0 ? true : false;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ConversationNode"/> class.
	/// </summary>
	/// <param name="dialogue">The dialogue to be contained in this node.</param>
	/// <param name="noOfResponses">The number of responses from this node.</param>
	public ConversationNode (string dialogue, int noOfResponses, float answeringDelay)
	{
		this._dialogue = dialogue;
		this._responses = new string[noOfResponses];
		this._nodeLinks = new int[noOfResponses];
		this._answeringDelay = answeringDelay;
	}

	public override string ToString ()
	{
		return string.Format ("[ConversationNode: responses={0}, nodeLinks={1}, Dialogue={2}]", _responses, _nodeLinks, _dialogue);
	}
	

}
