using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemy : MonoBehaviour
{

    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    public GameObject[] objectsToActivate;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        boxCollider.enabled = true;
        spriteRenderer.enabled = false;
        DeactivateObjects();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ActivateObjects();
            boxCollider.enabled = false;
        }
    }

    private void ActivateObjects()
    {
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(true);
        }
    }

    private void DeactivateObjects()
    {
        for (int j = 0; j < objectsToActivate.Length; j++)
        {

            objectsToActivate[j].SetActive(false);
        }
    }
    
}
