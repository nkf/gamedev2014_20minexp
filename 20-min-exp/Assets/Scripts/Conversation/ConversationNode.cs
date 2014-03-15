using UnityEngine;
using System.Collections;
using SimpleJSON;

public class ConversationNode {

	public string Dialogue;
	public string[] responses;
	
	// The indexes in this array points to indexes in an array which contains all conversation nodes.
	public int[] nodeLinks;

	/**
	 * Whether this node is an end node. This is currently true, if there are no responses available.
	 * This can be changed, if we want dialogues to be able to be "two-fold". I.e. the conversation partner
	 * is able have consectuive nodes (which would progress over time) without the need for a response from the player.
	 */
	public bool IsEndNode {
		get {
			return nodeLinks.Length == 0 ? true : false;
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ConversationNode"/> class.
	/// </summary>
	/// <param name="dialogue">The dialogue to be contained in this node.</param>
	/// <param name="noOfResponses">The number of responses from this node.</param>
	public ConversationNode (string dialogue, int noOfResponses)
	{
		Dialogue = dialogue;
		responses = new string[noOfResponses];
		nodeLinks = new int[noOfResponses];
	}

	public override string ToString ()
	{
		return string.Format ("[ConversationNode: responses={0}, nodeLinks={1}, Dialogue={2}]", responses, nodeLinks, Dialogue);
	}
	

}
