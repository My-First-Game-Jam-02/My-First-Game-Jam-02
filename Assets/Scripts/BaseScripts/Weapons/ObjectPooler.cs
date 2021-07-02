using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public Transform playerBulletHolder;
    public Transform enemyBulletHolder;
    public Transform laserHolder;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
                if(pool.tag == "PlayerBullets")
                {
                    obj.transform.SetParent(playerBulletHolder);
                }
                if(pool.tag == "EnemyBullets")
                {
                    obj.transform.SetParent(enemyBulletHolder);
                }

                if(pool.tag == "Lasers")
                {
                    obj.transform.SetParent(laserHolder);
                }
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Vector3 direction, Quaternion rotation)
    {

        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }
        
        //Dequeue returns and removes the first object in the que.
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if(pooledObject != null)
        {
            pooledObject.OnObjectSpawn(direction);
        }

        //Enqueue puts an object at the end of a que.
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

}
