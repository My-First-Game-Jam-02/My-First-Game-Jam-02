using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private bool isRespawningEnemy;
    private EnemyController enemyController;
    private EnemyHealth enemyHealth;

    public GameObject enemyToRespawn;
    public float timeBetweenRespawns;

    void Start()
    {
        enemyController = enemyToRespawn.GetComponent<EnemyController>();
        enemyHealth = enemyToRespawn.GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (enemyController.isDead && !isRespawningEnemy)
        {
            isRespawningEnemy = true;
            StartCoroutine(StartRespawn());
        }
    }

    private IEnumerator StartRespawn()
    {
        yield return new WaitForSeconds(timeBetweenRespawns);

        RespawnEnemy();
        isRespawningEnemy = false;
    }

    private void RespawnEnemy()
    {
        enemyController.PlaceAtOriginalPosition();
        enemyHealth.ResetHealth();
        enemyToRespawn.SetActive(true);
    }
}
