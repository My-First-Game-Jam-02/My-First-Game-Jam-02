using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : EnemyController
{

    public Vector3 targetPosition;
    public Transform targetTransform;

    [SerializeField] BaseContextSteering2D steering;
    public float timeBetweenTargeting;
    public float timeBetweenAttacks;
    public float startFollowDistance;
    public Vector2 targetOffest;
    public Transform gunEndPointPosition;
    [SerializeField] GameObject shootSfx;

    void OnEnable()
    {
        targetTransform = playerHealth.gameObject.transform;

        StartCoroutine(FindNewTargetPosition());
        StartCoroutine(DoIntervalAttack());
        timeBetweenAttacks = Random.Range(3f, timeBetweenAttacks);
    }

    public override void Update()
    {
        if (isPossessed)
        {
            return;
        }

        if (!isAttacking)
        {
            SetTarget();

            MoveTowardsTargetPosition();
            if (Vector2.Distance(new Vector2(targetTransform.position.x, targetTransform.position.y), transform.position) > startFollowDistance)
            {
                
            }

            SetFacingDirection();
        }
    }

    public void SetTarget()
    {
        if (playerController.isPossessing)
        {
            targetTransform = playerController.gameObject.transform;
        }
        else
        {
            targetTransform = playerHealth.gameObject.transform;
        }
    }

    public void MoveTowardsTargetPosition()
    {
        float step = speed * Time.deltaTime;
        Debug.DrawLine(transform.position, targetPosition, Color.cyan);

        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
    }

    public override void SetFacingDirection()
    {
        if(targetTransform.position.x < transform.position.x)
        {
            isFacingRight = false;
            transform.eulerAngles = new Vector3(0, 180f, 0);
        } else
        {
            isFacingRight = true;
            transform.eulerAngles = new Vector3(0, 0f, 0);
        }
    }

    private void SetTargetPosition()
    {
        Vector3 nextTargetPosition = Vector3.zero;
        float randomXOffset = Random.Range(0, 4f);
        float randomYOffset = Random.Range(0, 4f);

        if(playerController.isPlayer || playerController.isGuardBot || playerController.isSpirit)
        {
            if (targetTransform.position.x < transform.position.x)
            {
                nextTargetPosition = new Vector3(targetTransform.position.x + targetOffest.x + randomXOffset, targetTransform.position.y + targetOffest.y + randomYOffset, targetTransform.position.z);
            }
            else
            {
                nextTargetPosition = new Vector3(targetTransform.position.x - targetOffest.x - randomXOffset, targetTransform.position.y + targetOffest.y + randomYOffset, targetTransform.position.z);
            }
        }

        if (playerController.isDroneBot)
        {
            if (targetTransform.position.x < transform.position.x)
            {
                nextTargetPosition = new Vector3(targetTransform.position.x + targetOffest.x + randomXOffset, targetTransform.position.y + randomYOffset, targetTransform.position.z);
            }
            else
            {
                nextTargetPosition = new Vector3(targetTransform.position.x - targetOffest.x - randomXOffset, targetTransform.position.y + randomYOffset, targetTransform.position.z);
            }
        }

        targetPosition = nextTargetPosition;
    }

    private IEnumerator FindNewTargetPosition()
    {
        SetTargetPosition();

        yield return new WaitForSeconds(timeBetweenTargeting);

        StartCoroutine(FindNewTargetPosition());
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetBool("isShooting", true);
    }

    public void StopAttack()
    {
        isAttacking = false;
        animator.SetBool("isShooting", false);
    }

    private IEnumerator DoIntervalAttack()
    {
        Attack();

        yield return new WaitForSeconds(timeBetweenAttacks);

        StartCoroutine(DoIntervalAttack());
    }

    public void ShootBullet()
    {
        PlayShootSfx();
        Vector3 shootDir = (targetTransform.position - gunEndPointPosition.position).normalized;
        ObjectPooler.Instance.SpawnFromPool("EnemyBullets", gunEndPointPosition.position, shootDir, Quaternion.identity);
    }

    public void PlayShootSfx()
    {
        Instantiate(shootSfx, transform.position, transform.rotation);
    }
}
