using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    private int spawnedEnemies;

    public int maximumEnemies;
    public float timeBetweenSpawns;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(spawnedEnemies < maximumEnemies)
            {
                StartCoroutine(SpawnEnemy());
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator SpawnEnemy()
    {

        //Transform transformHenchPerson = Instantiate(henchPerson.transform, transform.position, Quaternion.identity);
        //ChasingNPC npc = transformHenchPerson.GetComponent<ChasingNPC>();
        //cablePlatformLevelController.AddEntity(npc);

        //NpcHenchpersonController npcHenchPerson = Instantiate(henchPerson, transform.position, Quaternion.identity);
        //npcHenchPerson.ChangeStateToChasePlayer();
        spawnedEnemies++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        if (spawnedEnemies < maximumEnemies)
        {
            StartCoroutine(SpawnEnemy());
        }
    }
}
