using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivateOnTask : MonoBehaviour
{

    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;

    public string taskToComplete;

    void Start()
    {
        if (QuestManager.Instance.CheckTaskComplete(taskToComplete))
        {
            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(true);
            }

            for (int j = 0; j < objectsToDeactivate.Length; j++)
            {

                objectsToDeactivate[j].SetActive(false);
            }
        }
    }
}
