using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSwitch : MonoBehaviour
{
    protected BoxCollider2D boxCollider;

    public Animator[] wallSegmentAnimators;

    // Start is called before the first frame update
    void Awake()
    {
        wallSegmentAnimators = GetComponentsInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateWall()
    {
        boxCollider.enabled = true;
        for(int i = 0; i < wallSegmentAnimators.Length; i++)
        {
            wallSegmentAnimators[i].SetBool("isActive", true);
        }
    }

    public void DeactivateWall()
    {
        boxCollider.enabled = false;
        for (int i = 0; i < wallSegmentAnimators.Length; i++)
        {
            wallSegmentAnimators[i].SetBool("isActive", false);
        }
    }
}
