using UnityEngine;
using System.Collections;

public class TestAgent : ConversationAgent {

	protected float _time;
	protected bool _IsDone = false;

	protected override void Start () {
		base.Start ();
		_time = Time.time;
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (Time.time - _time > 3 && !_IsDone)
		{
			_IsDone = true;
			StartConversation();
		}

	}
}
