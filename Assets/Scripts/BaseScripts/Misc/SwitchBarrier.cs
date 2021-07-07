using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBarrier : Switch
{

    public bool barrierActivated;
    public Animator[] laserBarrierAnimators;

    public override void Start()
    {
        base.Start();
        if (barrierActivated)
        {
            ActivateBarrier();
        }
        else
        {
            DeactivateBarrier();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3") && canBeUsed)
        {
            if (barrierActivated)
            {
                DeactivateBarrier();
            }
            else
            {
                ActivateBarrier();
            }
            
        }
    }

    private void DeactivateBarrier()
    {
        for (int i = 0; i < laserBarrierAnimators.Length; i++)
        {
            laserBarrierAnimators[i].SetBool("barrierOn", false);
        }
        
        animator.SetBool("isActive", false);
        barrierActivated = false;
    }

    private void ActivateBarrier()
    {
        for (int i = 0; i < laserBarrierAnimators.Length; i++)
        {
            laserBarrierAnimators[i].SetBool("barrierOn", true);
        }
        
        animator.SetBool("isActive", true);
        barrierActivated = true;
    }
}
