using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAction : MonoBehaviour
{

    private ActionController actionController;
    private SpriteRenderer spriteRenderer;

    public bool canBeActivated;

    public SingleAction actionToActivate;
    public bool showSprite;
    public bool isPlayerInputActivated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canBeActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canBeActivated = false;
        }
    }
    void Update()
    {
        if (isPlayerInputActivated && canBeActivated && Input.GetButtonDown("Fire1") && !GameManager.Instance.inventoryActive)
        {
            ActivateNextAction();
        }
        else if (!isPlayerInputActivated && canBeActivated)
        {
            ActivateNextAction();
            this.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        actionController = FindObjectOfType<ActionController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!showSprite && spriteRenderer != null){ spriteRenderer.enabled = false; }
         
    }

    public void ActivateNextAction()
    {
        actionController.DeactivateAllActions();
        actionToActivate.gameObject.SetActive(true);
    }
}
