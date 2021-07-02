using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [Tooltip("How long gameobject is active")]
    [SerializeField] float activeTime = 0.1f;
    float timeActiviated;
    float alpha;
    [SerializeField] float alphaSet = 0.8f;
    [Tooltip("How fast the sprite fades (low number = fades fastes)")]
    [SerializeField] float alphaMultiplier = 0.85f;

    Transform player;

    SpriteRenderer SR;
    SpriteRenderer playerSR;

    Color colour;

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActiviated = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        colour = new Color(1f, 1f, 1f, alpha);
        SR.color = colour;

        if (Time.time >= (timeActiviated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
