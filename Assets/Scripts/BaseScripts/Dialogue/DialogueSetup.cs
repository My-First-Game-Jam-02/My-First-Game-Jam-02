using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class DialogueSetup : MonoBehaviour
{

    private string jsonData;
    private DialogueManager dialogueManager;
    private SingleAction singleActionParent;

    public bool isActionDialogue = false;
    public bool isItemDialogue = false;
    public bool isPlayerCollisionTriggered = false;

    public string subFolder;
    public string dialogueFileName;
    public DialogueNodeHolder dialogueNodeHolder;
    public List<DialogueNode> dialogueNodeList = new List<DialogueNode>();
    public DialogueAction dialogueAction;
    public SingleAction singleAction;
    public NpcController npcController;

    void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        singleActionParent = GetComponentInParent<SingleAction>();
        dialogueAction = GetComponent<DialogueAction>();
    }

    void Start()
    {
        ConstructDialogue();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (!isActionDialogue && !isItemDialogue)
            {
                LoadDialogueIntoManager();
            }
            if (isPlayerCollisionTriggered)
            {
                StartDialogue();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RemoveDialogueFromManager();
        }
    }

    void LoadDialogueIntoManager()
    {
        bool dialogueAlreadyInList = false;
        for (int i = 0; i < dialogueManager.dialogueSetups.Count; i++)
        {
            if (dialogueManager.dialogueSetups[i] == this)
            {
                dialogueAlreadyInList = true;
            }
        }

        if (!dialogueAlreadyInList)
        {
            dialogueManager.AddDialogueToList(this);
        }

    }

    void RemoveDialogueFromManager()
    {
        dialogueManager.RemoveDialogueFromList(this);
    }

    public void ConstructDialogue()
    {

        TextAsset jsonTextFile = Resources.Load<TextAsset>("DialogueTree/" + subFolder + "/" + dialogueFileName);
        string jsonData = jsonTextFile.text;
        dialogueNodeHolder = JsonUtility.FromJson<DialogueNodeHolder>(jsonData);
        dialogueNodeList = dialogueNodeHolder.dialogueNodes;

        LoadSprites();
    }

    private void LoadSprites()
    {

        for (int i = 0; i < dialogueNodeList.Count; i++)
        {
            dialogueNodeList[i].speakerOneSprite = GetPortraitSprite(dialogueNodeList[i].speakerOnePortrait);
        }

        for (int i = 0; i < dialogueNodeList.Count; i++)
        {
            dialogueNodeList[i].centerPortraitSprite = GetPortraitSprite(dialogueNodeList[i].centerPortrait);
        }

        for (int t = 0; t < dialogueNodeList.Count; t++)
        {
            dialogueNodeList[t].speakerTwoSprite = GetPortraitSprite(dialogueNodeList[t].speakerTwoPortrait);
        }
    }

    public Sprite GetPortraitSprite(string portraitString)
    {
        for (int i = 0; i < dialogueManager.resourceSprites.Length; i++)
        {
            if (dialogueManager.resourceSprites[i].name == portraitString)
            {
                return dialogueManager.resourceSprites[i];
            }
        }
        return null;
    }

    public void StartDialogue()
    {

    }

    public void ActivateNextScene()
    {
        if (singleActionParent != null)
        {
            dialogueManager.isInActionDialogue = false;
            singleActionParent.isPlayingDialogue = false;
        }

        if (singleAction != null)
        {
            dialogueManager.isInActionDialogue = false;
            singleAction.isPlayingDialogue = false;
            singleAction.gameObject.SetActive(true);
        }
    }

    public void ActivateDialogueAction()
    {
        if (dialogueAction != null)
        {
            dialogueAction.PerformDialogueAction();
        }
    }
}

[System.Serializable]
public class DialogueNodeHolder
{
    public List<DialogueNode> dialogueNodes;
}

[System.Serializable]
public class DialogueNode
{
    public int id;
    public string spokenWords;
    public string[] replies;
    public int[] nodeLinks;
    public string speakerName;
    public string speakerOnePortrait;
    public string onePortraitFacingDirection;
    public string speakerTwoPortrait;
    public string twoPortraitFacingDirection;
    public string centerPortrait;
    public string voice;
    public string audioSFX;
    public string musicCue;
    public string overrideFacingDirectionPlayer;
    public string currentNpc;
    public string overrideFacingDirectionNPC;
    public string taskCompleted;

    public Sprite speakerOneSprite;
    public Sprite centerPortraitSprite;
    public Sprite speakerTwoSprite;

    public DialogueNode(int _id, string _spokenWords, string[] _replies, int[] _nodeLinks, string _speakerName, string _speakerOnePortrait, string _onePortraitFacingDirection, string _speakerTwoPortrait,
        string _twoPortraitFacingDirection, string _centerPortrait, string _voice, string _audioSFX, string _musicCue, string _overrideFacingDirectionPlayer, string _currentNpc, string _overrideFacingDirectionNPC,
        string _taskCompleted)
    {
        this.id = _id;
        this.spokenWords = _spokenWords;
        this.replies = _replies;
        this.nodeLinks = _nodeLinks;
        this.speakerName = _speakerName;
        this.speakerOnePortrait = _speakerOnePortrait;
        this.onePortraitFacingDirection = _onePortraitFacingDirection;
        this.speakerTwoPortrait = _speakerTwoPortrait;
        this.twoPortraitFacingDirection = _twoPortraitFacingDirection;
        this.centerPortrait = _centerPortrait;
        this.voice = _voice;
        this.audioSFX = _audioSFX;
        this.musicCue = _musicCue;
        this.overrideFacingDirectionPlayer = _overrideFacingDirectionPlayer;
        this.currentNpc = _currentNpc;
        this.overrideFacingDirectionNPC = _overrideFacingDirectionNPC;
        this.taskCompleted = _taskCompleted;
    }
}



