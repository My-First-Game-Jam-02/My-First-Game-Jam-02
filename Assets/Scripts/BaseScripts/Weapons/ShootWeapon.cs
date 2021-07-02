using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootWeapon : MonoBehaviour
{

    [SerializeField] private PlayerAim playerAim;
    [SerializeField] private Transform pfBullet;
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private GameObject shootSfx;

    private void Awake()
    {
        playerAim.OnShoot += PlayerShootProjectiles_OnShoot;
    }

    private void PlayerShootProjectiles_OnShoot(object sender, PlayerAim.OnShootEventArgs e)
    {
        Vector3 shootDir = (e.shootPosition - e.gunEndPointPosition).normalized;
        ObjectPooler.Instance.SpawnFromPool("PlayerBullets", e.gunEndPointPosition, shootDir, Quaternion.identity);
        gunAnimator.SetBool("isShooting", true);
        PlayShootSfx();
    }

    public void ResetGunAnimator()
    {
        gunAnimator.SetBool("isShooting", false);
    }

    public void PlayShootSfx()
    {
        Instantiate(shootSfx, transform.position, transform.rotation);
    }
}
