using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkTaskComplete : MonoBehaviour
{

    private QuestManager questManager;

    public string taskName;

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();    
    }

    void Update()
    {
        
    }

    public void SetTaskAsComplete()
    {
        questManager.SetTaskAsComplete(taskName);
    }
}
