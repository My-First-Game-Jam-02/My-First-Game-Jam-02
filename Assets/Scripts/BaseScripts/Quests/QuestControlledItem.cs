using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestControlledItem : MonoBehaviour
{

    private QuestManager questManager;
    private LevelControl levelControl;
    public bool killRequirementMet;
    public string questName;
    public GameObject[] objectsToActivate;
    public GameObject[] objectsToDeactivate;
    public bool hasKillRequirement;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            
        }   
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            AttemptToFinishQuest();

            if (CheckIfQuestComplete() && !hasKillRequirement)
            {
                ActivateAllObjects();
                DeactivateAllObjects();
            }
        }
    }

    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        levelControl = FindObjectOfType<LevelControl>();
    }

    public void AttemptToFinishQuest()
    {

    }


    public bool CheckIfQuestComplete()
    {
        bool questHasBeenCompleted = false;
        for (int i = 0; i < questManager.quests.Length; i++)
        {
            if(questName == questManager.quests[i].questName)
            {
                if (questManager.quests[i].hasBeenCompleted)
                {
                    questHasBeenCompleted = true;
                }
            }
        }

        return questHasBeenCompleted;
    }

    public void ActivateAllObjects()
    {
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(true);
        }
    }

    public void DeactivateAllObjects()
    {
        for (int i = 0; i < objectsToDeactivate.Length; i++)
        {
            objectsToDeactivate[i].SetActive(false);
        }
    }
}
