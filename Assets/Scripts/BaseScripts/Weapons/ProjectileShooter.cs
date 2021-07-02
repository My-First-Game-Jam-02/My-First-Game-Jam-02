using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour
{

    private float timeShotFired;
    private bool firstShotFired;
    private Animator animator;
    private ProximityActivatorForShooter proximityActivator;

    public Transform gunEndPointPosition;
    public enum ShootingDirection {UP, DOWN, LEFT, RIGHT}
    public string poolToSpawnFrom;
    public float timeBetweenShots;
    public float timeDelay;

    public ShootingDirection shootingDirection;

    public GameObject projectileSfx;

    void Start()
    {
        animator = GetComponent<Animator>();
        proximityActivator = GetComponentInParent<ProximityActivatorForShooter>();
        Invoke("ChangeStateToFiring", timeDelay);
    }

    void Update()
    {
        if(timeShotFired + timeBetweenShots < Time.time && firstShotFired)
        {
            ChangeStateToFiring();
        }
    }

    public void ChangeStateToFiring()
    {
        animator.SetBool("isShooting", true);
    }

    public void ChangeStateToIdle()
    {
        animator.SetBool("isShooting", false);
    }

    public void ShootProjectile()
    {
        timeShotFired = Time.time;
        firstShotFired = true;

        Vector3 shootDir = new Vector3(0, 1f, 0);
        switch (shootingDirection)
        {
            case ShootingDirection.UP:
                shootDir = new Vector3(0, 1f, 0);
                break;
            case ShootingDirection.DOWN:
                shootDir = new Vector3(0, -1f, 0);
                break;
            case ShootingDirection.LEFT:
                shootDir = new Vector3(1, 0f, 0);
                break;
            case ShootingDirection.RIGHT:
                shootDir = new Vector3(1, 0f, 0);
                break;
            default:
                break;
        }
        proximityActivator.TriggerShootingSfx();
        ObjectPooler.Instance.SpawnFromPool(poolToSpawnFrom, gunEndPointPosition.position, shootDir, Quaternion.identity);
    }

    private void PlayProjectileSfx()
    {
        Instantiate(projectileSfx, transform.position, transform.rotation);
    }
}
