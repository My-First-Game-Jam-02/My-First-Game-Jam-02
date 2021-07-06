using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{

    private Vector3 shootDir;
    public float speed;
    public int damageCaused;
    public bool playerBullet;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Wall")
        {
            gameObject.SetActive(false);
        }

        if (playerBullet)
        {
            if (collision.tag == "Enemy")
            {
                Health enemyHealth = collision.gameObject.GetComponent<Health>();
                enemyHealth.Damage(damageCaused);
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (collision.tag == "PlayerHealth")
            {
                SSPlayerHealth playerHealth = collision.gameObject.GetComponent<SSPlayerHealth>();
                playerHealth.Damage(damageCaused);
                gameObject.SetActive(false);
            }

            if(collision.tag == "Player")
            {
                PlayerEnemyHealth playerEnemyHealth = collision.gameObject.GetComponent<PlayerEnemyHealth>();
                playerEnemyHealth.Damage(damageCaused);
                gameObject.SetActive(false);
            }
        }
       
    }

    private void Update()
    {
        transform.position += shootDir * speed * Time.deltaTime;
    }

    public void OnObjectSpawn(Vector3 direction)
    {
        this.shootDir = direction;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(shootDir));
    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
