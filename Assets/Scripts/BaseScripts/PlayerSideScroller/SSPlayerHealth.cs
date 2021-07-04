using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class SSPlayerHealth : Health
{
    private SSPlayerController playerController;
    private Slider playerHealthBar;

    public GameObject playerHurtSFX;

    public override void Awake()
    {
        base.Awake();
        playerController = FindObjectOfType<SSPlayerController>();
        //playerHealthBar = GameObject.Find("health_bar").GetComponent<Slider>();
    }

    public override void Damage(int damageAmount)
    {
        if (isInvunerable || playerController.isFrozen)
        {
            return;
        }

        //currentHealth -= damageAmount;

        //if (currentHealth > 0)
        //{

        //    isInvunerable = true;
        //    Invoke("MakePlayerVulnerable", invincibilityTime);
        //    StopCoroutine(FlickerSprite());
        //    StartCoroutine(FlickerSprite());
        //    playerController.ChangeStateToStunned();
        //    PlayPlayerHurtSFX();
        //    UpdatePlayerHealthBar();
        //}
        //else
        //{
        //    PlayPlayerHurtSFX();
        //    KillPlayer();
        //}

    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdatePlayerHealthBar();
    }

    public void MakePlayerVulnerable()
    {
        isInvunerable = false;
        spriteRenderer.color = normalColor;
    }

    public IEnumerator FlickerSprite()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(filckerInterval);

        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

        yield return new WaitForSeconds(filckerInterval);

        if (isInvunerable)
        {
            StartCoroutine("FlickerSprite");
        }
    }

    public void KillPlayer()
    {
        currentHealth = 0;
        UpdatePlayerHealthBar();
        playerController.ChangeStateToDead();
    }

    public void PlayPlayerHurtSFX()
    {
        Instantiate(playerHurtSFX, transform.position, transform.rotation);
    }

    private void UpdatePlayerHealthBar()
    {
        int playerHealthValue = maxHealth - currentHealth;
        playerHealthBar.value = playerHealthValue;
    }

    public override void ResetHealth()
    {
        currentHealth = maxHealth;
        capsuleCollider.enabled = true;
    }
}
