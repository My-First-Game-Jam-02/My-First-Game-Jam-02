using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private SpriteRenderer itemPromptSpriteRenderer;
    private Animator itemPromptAnimator;
    private PlayerBehavior playerBehavior;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private MarkTaskComplete markTaskComplete;
    private DialogueManager dialogueManager;
    private CameraController cameraController;
    

    public bool canInteract;
    public bool canBeCollected;
    public bool hasBeenCollected;
    public string itemName;

    public GameObject itemPrompt;
   
    public GameObject player;

    public DialogueSetup dialogueSetup;
    public GameObject inventoryFullSFX;
    public GameObject itemCollectSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canBeCollected = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canBeCollected = false;
        }
    }

    void Start()
    {
        player = GameObject.Find("Player");
        dialogueManager = FindObjectOfType<DialogueManager>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        itemPromptSpriteRenderer = itemPrompt.GetComponent<SpriteRenderer>();
        itemPromptAnimator = itemPrompt.GetComponent<Animator>();
        playerBehavior = player.GetComponent<PlayerBehavior>();
        markTaskComplete = GetComponent<MarkTaskComplete>();
        cameraController = FindObjectOfType<CameraController>();
        if (GetComponent<PickupObject>())
        {
            itemName = GetComponent<PickupObject>().itemName;
        }

        if (dialogueSetup != null)
        {
            dialogueSetup.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //ShowAndHideItemPrompt();
        DetermineClosestItemToPlayer();
        RemoveItemOnPlayerExit();
        CheckToCollectItem();
    }

    public void ShowAndHideItemPrompt()
    {
        if (canBeCollected && canInteract && playerBehavior.close == gameObject)
        {
            itemPromptSpriteRenderer.enabled = true;
        }
        else if (playerBehavior.close != gameObject || !canInteract || !canBeCollected)
        {
            itemPromptSpriteRenderer.enabled = false;
        }
    }

    public void DetermineClosestItemToPlayer()
    {
        if (canBeCollected && canInteract && playerBehavior.interactEnable == true)
        {
            if (playerBehavior.close == null)
            {
                playerBehavior.close = gameObject;
            }

            if (playerBehavior.close != null && playerBehavior.close != gameObject)
            {
                if (Vector2.Distance(transform.position, player.transform.position) < Vector2.Distance(playerBehavior.close.transform.position, player.transform.position))
                {
                    playerBehavior.close = gameObject;
                }
            }
        }
    }

    public void RemoveItemOnPlayerExit()
    {
        if (!canBeCollected && playerBehavior.close == gameObject)
        {
            playerBehavior.close = null;
        }
    }

    public void CheckToCollectItem()
    {

        if(GameManager.Instance.pauseMenuActive || GameManager.Instance.inventoryActive)
        {
            return;
        }

        if (!hasBeenCollected && canBeCollected && (Input.GetButtonDown("Fire1")) && canInteract && playerBehavior.interactEnable && playerBehavior.close == gameObject)
        {

            if (inventoryManager.CheckInventoryFull())
            {
                PlayInventoryFullSFX();
            } else
            {
                inventoryManager.PickUp(itemName);
                spriteRenderer.enabled = false;
                hasBeenCollected = true;
                PlayItemCollectSFX();
                if(dialogueSetup != null)
                {
                    dialogueSetup.gameObject.SetActive(true);
                    PlayDialogueImmediately();
                }
                

                if (markTaskComplete != null)
                {
                    markTaskComplete.SetTaskAsComplete();
                }
            }
        }
    }

    public void PlayInventoryFullSFX()
    {
        Instantiate(inventoryFullSFX, transform.position, transform.rotation);
    }

    public void PlayItemCollectSFX()
    {
        Instantiate(itemCollectSFX, transform.position, transform.rotation);
    }

    public void PlayDialogueImmediately()
    {
        dialogueManager.ClearAllDialogueFromList();
        dialogueSetup.ConstructDialogue();
        dialogueManager.currentDialogueSetup = dialogueSetup;
        dialogueManager.ActivateDialogue();
    }
}
