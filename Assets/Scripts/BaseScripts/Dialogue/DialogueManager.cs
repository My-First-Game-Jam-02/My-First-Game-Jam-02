using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    private SSPlayerController playerController;
    private bool isOpening;
    private AudioSource audioSource;
    private bool isReplying;
    private LevelControl levelController;

    public bool isInActionDialogue;
    public bool isInItemDialogue;
    public int replyNodeID;

    public Sprite[] resourceSprites { get; private set; }
    public bool dialogueActive { get; private set; }
    public bool isTyping { get; private set; }
    public DialogueSetup currentDialogueSetup;
    public DialogueNode currentDialogueNode;
    public List<DialogueSetup> dialogueSetups = new List<DialogueSetup>();

    [Header("Settings")]
    [SerializeField] private AudioClip typeSound;

    public float typingSpeed = 0.01f;
    public float typeSoundSpeed = 0.1f;
    public float typeSoundVolume = 0.2f;
    public float musicFadeTime = 1f;

    [Header("Objects")]
    public GameObject replyHolder;
    public Button replyButton;
    public GameObject dialogueHolder;
    public Animator dialogueHolderAnimator;
    public Image portraitImageOne;
    public Image centerPortraitImage;
    public Image portraitImageTwo;
    public RectTransform portraitRectOne;
    public RectTransform portraitRectTwo;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public AudioClip maleVoice;
    public AudioClip femaleVoice;
    public AudioClip genericVoice;

    void Awake()
    {
        playerController = FindObjectOfType<SSPlayerController>();
        dialogueHolderAnimator = dialogueHolder.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        levelController = FindObjectOfType<LevelControl>();

        LoadAllSprites();
        dialogueHolder.SetActive(false);
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1") && !isOpening)
        {
            //This code runs for normal dialogue interactions when the player talks with NPCs.
            if (dialogueSetups.Count > 0 && !isInActionDialogue && !isInItemDialogue)
            {
                if (dialogueActive && !isReplying)
                {
                    if (isTyping)
                    {
                        FastForwardDialogue();
                    }
                    else
                    {
                        DisplayNextDialogueLine();
                    }
                }

                if (!dialogueActive)
                {
                    FindClosestDialogue();
                    ActivateDialogue();
                }
            }

            // This code runs when a action controlled dialogue or item dialogue is loaded and played.
            if (currentDialogueSetup != null)
            {
                if (isInActionDialogue || isInItemDialogue)
                {
                    if (dialogueActive && !isReplying)
                    {
                        if (isTyping)
                        {
                            FastForwardDialogue();
                        }
                        else
                        {
                            DisplayNextDialogueLine();
                        }
                    }

                    if (!dialogueActive)
                    {
                        ActivateDialogue();
                    }
                }
            }
        }
    }

    public void ActivateDialogue()
    {
        dialogueActive = true;
        dialogueText.text = "";
        playerController.ChangeStateToFrozen();
        currentDialogueNode = currentDialogueSetup.dialogueNodeList[0];
        dialogueHolder.SetActive(true);
        GameManager.Instance.dialogueActive = true;
        StartOpeningDialogueHolder();
        MakeSpeakersFaceEachOther();
        OverrideFacingDirection();
        SetSpeakerName();
        SetPortraitSprite();
        SetPortraitDirection();
    }

    public void FindClosestDialogue()
    {
        DialogueSetup closestDialogueSetup = null;
        float currentClosestDistance = 10000;

        for (int i = 0; i < dialogueSetups.Count; i++)
        {
            if (currentClosestDistance == 10000)
            {
                currentClosestDistance = Vector3.Distance(playerController.gameObject.transform.position, dialogueSetups[i].gameObject.transform.position);
                closestDialogueSetup = dialogueSetups[i];
            }
            else
            {
                float distance = Vector3.Distance(playerController.gameObject.transform.position, dialogueSetups[i].gameObject.transform.position);
                if (distance < currentClosestDistance)
                {
                    currentClosestDistance = distance;
                    closestDialogueSetup = dialogueSetups[i];
                }
            }
        }

        SetAsCurrentDialogueSetup(closestDialogueSetup);
    }

    public void SetAsCurrentDialogueSetup(DialogueSetup dialogueSetup)
    {
        currentDialogueSetup = dialogueSetup;
    }

    public void StartDialogue()
    {
        isOpening = false;
        Dialogue();
    }

    public DialogueNode FindNextDialogueNode()
    {
        DialogueNode nextDialogueNode = null;

        if (currentDialogueNode != null)
        {
            if (currentDialogueNode.nodeLinks.Length > 0)
            {
                // When a player chooses a reply, this will return the correct dialogueNode that matches the reply.
                if (isReplying)
                {
                    for (int i = 0; i < currentDialogueSetup.dialogueNodeList.Count; i++)
                    {
                        if (currentDialogueSetup.dialogueNodeList[i].id == replyNodeID)
                        {
                            nextDialogueNode = currentDialogueSetup.dialogueNodeList[i];
                        }
                    }
                }
                else
                {
                    // This will compare all the dialogueNode ids to the next nodeLink, find the matching dialogueNode, then set this as the nextDialogueNode.
                    for (int i = 0; i < currentDialogueSetup.dialogueNodeList.Count; i++)
                    {
                        if (currentDialogueSetup.dialogueNodeList[i].id == currentDialogueNode.nodeLinks[0])
                        {
                            nextDialogueNode = currentDialogueSetup.dialogueNodeList[i];

                            // If the next dialogue node has replies, this will activate the reply holder and create the reply buttons.
                            if (nextDialogueNode.replies.Length > 0)
                            {
                                isReplying = true;
                                replyHolder.SetActive(true);
                                dialogueText.gameObject.SetActive(false);
                                CreateReplyButtons(nextDialogueNode);
                            }
                        }
                    }
                }
            }
        }

        return nextDialogueNode;
    }

    public void CreateReplyButtons(DialogueNode dialogueNode)
    {
        for (int i = 0; i < dialogueNode.replies.Length; i++)
        {
            Button newButton = Instantiate(replyButton, replyHolder.transform);
            ReplyButton newReplyButton = newButton.GetComponent<ReplyButton>();
            newReplyButton.SetReplyButtonText(dialogueNode.replies[i]);
            newReplyButton.SetNodeLink(dialogueNode.nodeLinks[i]);
            if (i == 0)
            {
                newButton.Select();
                newButton.OnSelect(null);
            }
        }
    }

    public void DestroyReplyButtons()
    {
        Button[] replyButtons = replyHolder.GetComponentsInChildren<Button>();
        for (int i = 0; i < replyButtons.Length; i++)
        {
            Destroy(replyButtons[i].gameObject);
        }
    }

    public void Reply(int nodeLink)
    {
        replyNodeID = nodeLink;
        replyHolder.SetActive(false);
        DestroyReplyButtons();
        Invoke("StartNextDialogueAfterReply", 0.1f);
    }

    public void StartNextDialogueAfterReply()
    {
        dialogueText.gameObject.SetActive(true);
        DisplayNextDialogueLine();
        isReplying = false;
    }

    public void Dialogue()
    {
        dialogueText.text = "";
        OverrideFacingDirection();
        SetSpeakerName();
        SetPortraitSprite();
        SetPortraitDirection();
        MarkTaskAsComplete();
        PlayOneShotSFX();
        SwitchMusic();
        StartCoroutine(TypeSentence(currentDialogueNode.spokenWords));

    }

    public void MakeSpeakersFaceEachOther()
    {
        if (currentDialogueSetup.gameObject.transform.position.x > playerController.gameObject.transform.position.x)
        {
            playerController.MakePlayerFaceRight();
        }
        else
        {
            playerController.MakePlayerFaceLeft();
        }
    }

    public void OverrideFacingDirection()
    {
        if (currentDialogueNode.overrideFacingDirectionPlayer == "left")
        {
            playerController.MakePlayerFaceLeft();
        }

        if (currentDialogueNode.overrideFacingDirectionPlayer == "right")
        {
            playerController.MakePlayerFaceRight();
        }

        if (!string.IsNullOrEmpty(currentDialogueNode.currentNpc))
        {
            GameObject npc = GameObject.Find(currentDialogueNode.currentNpc);
            if (npc != null)
            {
                NpcController npcController = npc.GetComponent<NpcController>();
                if (currentDialogueNode.overrideFacingDirectionNPC == "left")
                {
                    npcController.MakeNpcFaceLeft();
                }
                if (currentDialogueNode.overrideFacingDirectionNPC == "right")
                {
                    npcController.MakeNpcFaceRight();
                }
            }
            else
            {
                print("npc not found");
            }
        }
    }

    public void SetSpeakerName()
    {
        if (!string.IsNullOrEmpty(currentDialogueNode.speakerName))
        {
            speakerNameText.text = currentDialogueNode.speakerName;
        }
        else
        {
            speakerNameText.text = "";
        }
    }

    public void SetPortraitSprite()
    {
        if (currentDialogueNode.speakerOneSprite != null)
        {
            portraitImageOne.gameObject.SetActive(true);
            portraitImageOne.sprite = currentDialogueNode.speakerOneSprite;
        }
        else
        {
            portraitImageOne.gameObject.SetActive(false);
        }

        if (currentDialogueNode.centerPortraitSprite != null)
        {
            centerPortraitImage.gameObject.SetActive(true);
            centerPortraitImage.sprite = currentDialogueNode.centerPortraitSprite;
        }
        else
        {
            centerPortraitImage.gameObject.SetActive(false);
        }

        if (currentDialogueNode.speakerTwoSprite != null)
        {
            portraitImageTwo.gameObject.SetActive(true);
            portraitImageTwo.sprite = currentDialogueNode.speakerTwoSprite;
        }
        else
        {
            portraitImageTwo.gameObject.SetActive(false);
        }
    }

    public void SetPortraitDirection()
    {
        if (currentDialogueNode.onePortraitFacingDirection == "left")
        {
            portraitRectOne.localScale = new Vector3(-Mathf.Abs(portraitRectOne.localScale.x), portraitRectOne.localScale.y, 1f);
        }
        else if (currentDialogueNode.onePortraitFacingDirection == "right")
        {
            portraitRectOne.localScale = new Vector3(Mathf.Abs(portraitRectOne.localScale.x), portraitRectOne.localScale.y, 1f);
        }

        if (currentDialogueNode.twoPortraitFacingDirection == "left")
        {
            portraitRectTwo.localScale = new Vector3(-Mathf.Abs(portraitRectTwo.localScale.x), portraitRectTwo.localScale.y, 1f);
        }
        else if (currentDialogueNode.twoPortraitFacingDirection == "right")
        {
            portraitRectTwo.localScale = new Vector3(Mathf.Abs(portraitRectTwo.localScale.x), portraitRectTwo.localScale.y, 1f);
        }
    }

    public void SetSpeakerVoice()
    {
        if (currentDialogueNode == null)
        {
            return;
        }

        switch (currentDialogueNode.voice)
        {
            case "male":
                audioSource.clip = maleVoice;
                break;
            case "female":
                audioSource.clip = femaleVoice;
                break;
            case "generic":
                audioSource.clip = genericVoice;
                break;
            default:
                break;
        }
    }

    public void MarkTaskAsComplete()
    {
        if (currentDialogueNode.taskCompleted != "")
        {
            print(currentDialogueNode.taskCompleted + " is complete");
            QuestManager.Instance.SetTaskAsComplete(currentDialogueNode.taskCompleted);
            currentDialogueSetup.ActivateDialogueAction();
        }
    }

    public void PlayOneShotSFX()
    {
        string audioSFX = currentDialogueNode.audioSFX;
        if (audioSFX != "")
        {
            GameObject audioOneShot = Resources.Load("AudioPrefabs/" + audioSFX) as GameObject;
            if (audioOneShot != null)
            {
                Instantiate(audioOneShot, transform.position, transform.rotation);
            }
        }
    }

    public void SwitchMusic()
    {
        string musicCue = currentDialogueNode.musicCue;
        if (musicCue != "")
        {
            SoundManager.Instance.FadeAndPlayMusic(musicCue, musicFadeTime);
        }
    }

    public void DisplayNextDialogueLine()
    {
        currentDialogueNode = FindNextDialogueNode();
        if (currentDialogueNode == null)
        {
            EndDialogue();
            return;
        }

        Dialogue();
    }

    public IEnumerator TypeSentence(string spokenWords)
    {
        isTyping = true;
        int characterIndex = 0;
        SetSpeakerVoice();

        if (spokenWords.Length > 0)
        {
            StartCoroutine(PlayTypingSound());
        }

        for (int i = 0; i <= spokenWords.Length - 1; i++)
        {
            characterIndex++;
            string text = spokenWords.Substring(0, characterIndex);
            text += "<color=#00000000>" + spokenWords.Substring(characterIndex) + "</color>";
            dialogueText.text = text;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }

        isTyping = false;
    }

    IEnumerator PlayTypingSound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(typeSoundSpeed);
        if (isTyping)
        {
            StartCoroutine(PlayTypingSound());
        }
    }

    public void FastForwardDialogue()
    {
        StopAllCoroutines();
        isTyping = false;
        dialogueText.text = currentDialogueNode.spokenWords;
    }

    public void EndDialogue()
    {
        StartClosingDialogueHolder();
    }

    public void CloseDialogueHolder()
    {

        if (!currentDialogueSetup.isActionDialogue)
        {
            playerController.ChangeStateToIdle();
        }
        else
        {
            currentDialogueSetup.ActivateNextScene();
            ClearAllDialogueFromList();
        }

        DialogueAction dialogueAction = currentDialogueSetup.gameObject.GetComponent<DialogueAction>();
        if (dialogueAction != null)
        {
            dialogueAction.PerformDialogueAction();
        }

        dialogueActive = false;
        currentDialogueSetup = null;
        currentDialogueNode = null;
        isInActionDialogue = false;
        isInItemDialogue = false;
        GameManager.Instance.dialogueActive = false;
        dialogueHolder.SetActive(false);
    }

    private void LoadAllSprites()
    {
        resourceSprites = Resources.LoadAll<Sprite>("Portraits");
    }

    public void AddDialogueToList(DialogueSetup dialogueSetup)
    {
        dialogueSetups.Add(dialogueSetup);
    }

    public void RemoveDialogueFromList(DialogueSetup dialogueSetup)
    {
        dialogueSetups.Remove(dialogueSetup);
    }

    public void ClearAllDialogueFromList()
    {
        dialogueSetups.Clear();
    }

    public void StartOpeningDialogueHolder()
    {
        dialogueHolderAnimator.SetBool("isClosing", false);
        isOpening = true;
        GameManager.Instance.dialogueActive = true;
        isInActionDialogue = currentDialogueSetup.isActionDialogue;
        isInItemDialogue = currentDialogueSetup.isItemDialogue;
    }

    public void StartClosingDialogueHolder()
    {
        dialogueHolderAnimator.SetBool("isClosing", true);
    }

}

