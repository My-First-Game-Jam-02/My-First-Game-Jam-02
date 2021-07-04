using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    protected Color normalColor;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected CapsuleCollider2D capsuleCollider;

    public int maxHealth;
    public int currentHealth;

    [Header("Settings")]
    public Color damageColor;
    public float invincibilityTime;
    public float filckerInterval;
    public bool isInvunerable;

    public virtual void OnEnable()
    {
        ResetHealth();
        if(capsuleCollider != null) { capsuleCollider.enabled = true; }
    }

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        normalColor = Color.white;
        currentHealth = maxHealth; 
    }

    void Update()
    {
        
    }

    public virtual void Damage(int damageAmount)
    {

    }

    public virtual void Kill()
    {

    }

    public virtual void DeactivateObject()
    {
        //this.gameObject.SetActive(false);
    }

    public virtual void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public virtual IEnumerator FlickerImage()
    {
        spriteRenderer.color = damageColor;

        yield return new WaitForSeconds(filckerInterval);

        spriteRenderer.color = normalColor;

        yield return new WaitForSeconds(filckerInterval);

        if (isInvunerable)
        {
            StartCoroutine("FlickerImage");
        }
    }
}
