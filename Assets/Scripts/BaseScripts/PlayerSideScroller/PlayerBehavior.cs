using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    public bool interactEnable;
    public GameObject close;

    [Header("Saved Stats")]
    public int health;
    public int currentSceneIndex;
    public Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = gameObject.scene.buildIndex;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = transform.position;
    }
}
