using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    private EnemyController enemyController;
    private Rigidbody2D rigidbodyEnemy;
    public LayerMask enemyLayerMask;
    public LayerMask deadLayerMask;
    public GameObject hurtSfx;

    public override void OnEnable()
    {
        base.OnEnable();
        enemyController = GetComponent<EnemyController>();
        rigidbodyEnemy = GetComponent<Rigidbody2D>();
        gameObject.layer = 7;
    }

    public override void Damage(int damageAmount)
    {

        if (isInvunerable)
        {
            return;
        }

        PlayHurtSfx();
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Kill();
            return;
        }

        isInvunerable = true;
        animator.SetBool("isDamaged", true);
        StopCoroutine(FlickerImage());
        StartCoroutine(FlickerImage());
        Invoke("MakeEnemyVulnerable", invincibilityTime);
    }

    public override void Kill()
    {
        animator.SetBool("isDead", true);
        gameObject.layer = 12;
        rigidbodyEnemy.gravityScale = 1f;
        rigidbodyEnemy.bodyType = RigidbodyType2D.Dynamic;

        if (enemyController != null)
        {
            enemyController.ChangeStateToDead();
        }
    }

    public void MakeEnemyVulnerable()
    {
        isInvunerable = false;
        spriteRenderer.color = normalColor;
    }

    public void PlayHurtSfx()
    {
        Instantiate(hurtSfx, transform.position, transform.rotation);
    }
}
