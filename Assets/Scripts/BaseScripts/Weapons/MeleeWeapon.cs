using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public int damageAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            enemyHealth.Damage(damageAmount);
        }
    }
}
