using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReplyButton : MonoBehaviour
{
    private DialogueManager dialogueManager;
    public Text relpyButtonText;
    public int nodeLink;
    
    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    public void SetReplyButtonText(string replyText)
    {
        relpyButtonText.text = replyText;
    }

    public void SetNodeLink(int nodeLink)
    {
        this.nodeLink = nodeLink;
    }

    public void Reply()
    {
        dialogueManager.Reply(nodeLink);
    }
}
