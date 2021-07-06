using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemyHealth : Health
{

    private SSPlayerController playerController;
    private bool isPossessing;
    public GameObject hurtSfx;
    public EnemyHealth possessedEnemyHealth;

    public override void Awake()
    {
        playerController = FindObjectOfType<SSPlayerController>();
        base.Awake();
    }

    public override void Damage(int damageAmount)
    {

        if (isInvunerable || !isPossessing)
        {
            return;
        }

        PlayHurtSfx();
        currentHealth -= damageAmount;
        possessedEnemyHealth.currentHealth = currentHealth;
        if (currentHealth <= 0)
        {
            KillPossessedEnemy();
            return;
        }

        isInvunerable = true;
        StopCoroutine(FlickerImage());
        StartCoroutine(FlickerImage());
        Invoke("MakeEnemyVulnerable", invincibilityTime);
    }

    public void KillPossessedEnemy()
    {
        currentHealth = 0;
        if(possessedEnemyHealth != null)
        {
            possessedEnemyHealth.currentHealth = currentHealth;
        }
        playerController.DepossessEnemy();
    }

    public void MakeEnemyVulnerable()
    {
        isInvunerable = false;
        spriteRenderer.color = normalColor;
    }

    public void SetHealthToPossessedEnemyHealth(EnemyHealth enemyHealth)
    {
        isPossessing = true;
        possessedEnemyHealth = enemyHealth;
        currentHealth = possessedEnemyHealth.currentHealth;
    }

    public void ReleasePossessedEnemyHealth()
    {
        isPossessing = false;
        currentHealth = 0;
        possessedEnemyHealth = null;
        StopCoroutine(FlickerImage());
    }

    public void PlayHurtSfx()
    {
        Instantiate(hurtSfx, transform.position, transform.rotation);
    }
}
