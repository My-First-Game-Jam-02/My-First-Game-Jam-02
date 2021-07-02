using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffSpriteRenderers : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].enabled = false;
        }
    }
}
