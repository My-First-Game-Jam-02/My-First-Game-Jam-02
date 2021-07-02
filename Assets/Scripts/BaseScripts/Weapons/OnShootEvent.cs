using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnShootEvent : MonoBehaviour
{

    [SerializeField] private PlayerAim playerAim;

    void Start()
    {
        playerAim.OnShoot += PlayerAim_OnShoot;
    }

    private void PlayerAim_OnShoot(object sender, PlayerAim.OnShootEventArgs e)
    {

    }
}
