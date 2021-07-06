using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBarrier : Switch
{

    public bool barrierActivated;
    public Animator laserBarrierAnimator;

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
        laserBarrierAnimator.SetBool("barrierOn", false);
        animator.SetBool("isActive", false);
        barrierActivated = false;
    }

    private void ActivateBarrier()
    {
        laserBarrierAnimator.SetBool("barrierOn", true);
        animator.SetBool("isActive", true);
        barrierActivated = true;
    }
}
