using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{

    [SerializeField]
    private Vector3 offSet = new Vector3(1f, 1f, 100f);

    private SpriteRenderer spriteRenderer;
    private SSPlayerController playerController;

    public Sprite defaultCursor;

    void Start()
    {
        playerController = FindObjectOfType<SSPlayerController>();
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultCursor;
    }

    void Update()
    {

        if (!playerController.isDroneBot)
        {
            spriteRenderer.enabled = false;
            Cursor.visible = true;
        }
        else
        {
            spriteRenderer.enabled = true;
            Cursor.visible = false;
        }

        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = cursorPosition + offSet;
    }
}