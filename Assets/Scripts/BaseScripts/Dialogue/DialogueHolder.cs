using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour
{

    private DialogueManager dialogueManager;

    void Awake()
    {
        dialogueManager = GetComponentInParent<DialogueManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartDialogue()
    {
        dialogueManager.StartDialogue();
    }

    public void CloseDialogueHolder()
    {
        dialogueManager.CloseDialogueHolder();
    }
}
