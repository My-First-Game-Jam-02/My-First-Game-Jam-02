using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{

    private Vector3 targetPosition;
    private SSPlayerController playerController;
    private bool isFacingRight;
    private Transform playerTransform;
    private bool isAttacking;
    private Animator animator;
    private bool isFacingDown;

    [SerializeField] BaseContextSteering2D steering;
    public float speed;
    public float timeBetweenTargeting;
    public float timeBetweenAttacks;
    public float startFollowDistance;
    public Vector2 targetOffest;
    public float facedownDistance;
    public Transform gunEndPointPosition;
    [SerializeField] GameObject shootSfx;

    void Start()
    {
        playerController = FindObjectOfType<SSPlayerController>();
        playerTransform = playerController.gameObject.transform;
        animator = GetComponent<Animator>();

        StartCoroutine(FindNewTargetPosition());
        StartCoroutine(DoIntervalAttack());
    }

    void Update()
    {
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
        Vector3 targetpoint = steering.MoveDirection();
        Debug.DrawLine(transform.position, transform.position + targetpoint, Color.cyan);

        
        transform.position = Vector3.MoveTowards(transform.position, transform.position + targetpoint, step);
    }

    private void SetFacingDirection()
    {
        if(playerTransform.position.x < transform.position.x)
        {
            isFacingRight = false;
            transform.eulerAngles = new Vector3(0, 0, 0);
            animator.SetBool("isFacingDown", false);
        } else
        {
            isFacingRight = true;
            transform.eulerAngles = new Vector3(0, 180f, 0);
            animator.SetBool("isFacingDown", false);
        }

        transform.localScale = new Vector3(1f, 1f, 1f);
        isFacingDown = false;

        if (Mathf.Abs(playerTransform.position.x - transform.position.x) < facedownDistance)
        {
            isFacingDown = true;
            animator.SetBool("isFacingDown", true);
            if(playerTransform.position.y > transform.position.y)
            {
                transform.localScale = new Vector3(1f, -1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
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
