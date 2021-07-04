using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthFollow : MonoBehaviour
{
    private bool shouldFollowPlayer;

    public GameObject player;
    public GameObject anchor;

    void Start()
    {
        
    }

    void Update()
    {
        if (!shouldFollowPlayer)
        {
            transform.position = anchor.transform.position;
        } else
        {
            transform.position = player.transform.position;
        }
    }

    public void MakeHealthFollowPlayer()
    {
        shouldFollowPlayer = true;
    }

    public void MakeHealthFollowAnchor()
    {
        shouldFollowPlayer = false;
    }
}
