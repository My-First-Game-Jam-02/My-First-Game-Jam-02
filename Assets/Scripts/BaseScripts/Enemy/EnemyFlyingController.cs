using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : EnemyController
{

    private Vector3 targetPosition;
    private Transform playerTransform;

    [SerializeField] BaseContextSteering2D steering;
    public float timeBetweenTargeting;
    public float timeBetweenAttacks;
    public float startFollowDistance;
    public Vector2 targetOffest;
    public Transform gunEndPointPosition;
    [SerializeField] GameObject shootSfx;

    void Start()
    {
        playerTransform = playerHealth.gameObject.transform;

        StartCoroutine(FindNewTargetPosition());
        StartCoroutine(DoIntervalAttack());
    }

    public override void Update()
    {
        if (isPossessed)
        {
            return;
        }

        if (!isAttacking)
        {

            if(Vector2.Distance(new Vector2(playerTransform.position.x,playerTransform.position.y), transform.position) > startFollowDistance)
            {
                MoveTowardsTargetPosition();
            }

            SetFacingDirection();
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
        if(playerTransform.position.x < transform.position.x)
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
        Vector3 nextTargetPosition;

        if (playerTransform.position.x < transform.position.x)
        {
            if(playerTransform.position.y < transform.position.y)
            {
                nextTargetPosition = new Vector3(playerTransform.position.x + targetOffest.x, playerTransform.position.y + targetOffest.y, playerTransform.position.z);
            }
            else
            {
                nextTargetPosition = new Vector3(playerTransform.position.x + targetOffest.x, playerTransform.position.y - (targetOffest.y), playerTransform.position.z);
            }
        }
        else
        {
            if (playerTransform.position.y < transform.position.y)
            {
                nextTargetPosition = new Vector3(playerTransform.position.x - targetOffest.x, playerTransform.position.y + targetOffest.y, playerTransform.position.z);
            }
            else
            {
                nextTargetPosition = new Vector3(playerTransform.position.x - targetOffest.x, playerTransform.position.y - (targetOffest.y), playerTransform.position.z);
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
        Vector3 shootDir = (playerTransform.position - gunEndPointPosition.position).normalized;
        ObjectPooler.Instance.SpawnFromPool("EnemyBullets", gunEndPointPosition.position, shootDir, Quaternion.identity);
    }

    public void PlayShootSfx()
    {
        Instantiate(shootSfx, transform.position, transform.rotation);
    }
}
