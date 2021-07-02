using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{

    [SerializeField]
    private Vector3 offSet = new Vector3(1f, 1f, 100f);

    private SpriteRenderer rend;

    public Sprite cursorClicking;
    public Sprite defaultCursor;

    void Start()
    {
        Cursor.visible = false;
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = defaultCursor;
    }

    void Update()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.position = cursorPosition + offSet;



        if (Input.GetMouseButtonDown(0))
        {
            rend.sprite = cursorClicking;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rend.sprite = defaultCursor;
        }
    }
}