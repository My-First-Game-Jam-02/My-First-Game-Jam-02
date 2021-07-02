using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    [SerializeField] GameObject afterImagePrefab;
    private SSPlayerController playerController;
    public int numberOfAfterImages;

    // Used to store all inactive gameobjects
    Queue<GameObject> availableObjects = new Queue<GameObject>();

    // Singleton
    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        playerController = FindObjectOfType<SSPlayerController>();
        GrowPool();
    }

    // Used to create more game objects for the pool
    private void GrowPool()
    {
        for (int i = 0; i < numberOfAfterImages; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    // Used to add created gameobjects to pool
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }

        // Take game object from pool
        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        if (playerController.antiGravityOn)
        {
            instance.transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            instance.transform.localScale = new Vector3(1, 1, 1);
        }
        return instance;
    }
}
