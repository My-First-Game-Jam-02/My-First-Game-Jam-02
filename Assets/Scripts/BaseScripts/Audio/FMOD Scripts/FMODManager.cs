using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODManager : MonoBehaviour
{
	public static FMODManager instance;

	void Awake()
	{
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
