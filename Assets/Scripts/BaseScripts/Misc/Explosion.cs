using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    
    void Start()
    {
        Invoke("DestroyGameObject", 1f);
    }

    void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
