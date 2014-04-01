using UnityEditor;
using UnityEngine;
using System.Collections;

public class WifeAgent : ConversationAgent {
    protected override void StopConversation() {
        base.StopConversation();
        Toolbox.Instance.levelController.LoadNext(10);
    }
}
