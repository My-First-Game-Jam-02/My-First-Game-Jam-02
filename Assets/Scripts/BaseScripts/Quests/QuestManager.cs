using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public static QuestManager Instance;
    public Task[] tasks;
    public Quest[] quests;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void SetTaskAsComplete(string taskName)
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            if (taskName == tasks[i].taskName)
            {
                tasks[i].hasBeenCompleted = true;
            }
        }

        CheckQuestCompletion();
    }

    public bool CheckTaskComplete(string taskName)
    {
        bool taskComplete = false;
        for (int i = 0; i < tasks.Length; i++)
        {
            if (taskName == tasks[i].taskName)
            {
                if (tasks[i].hasBeenCompleted == true)
                {
                    taskComplete = true;
                }
            }
        }

        return taskComplete;
    }

    public void CheckQuestCompletion()
    {
        for (int i = 0; i < quests.Length; i++)
        {
            if (!quests[i].hasBeenCompleted)
            {
                Quest unCompletedQuest = quests[i];
                bool allTasksCompleted = true;
                for (int t = 0; t < unCompletedQuest.tasksNeededForCompletion.Length; t++)
                {
                    for (int s = 0; s < tasks.Length; s++)
                    {
                        if (unCompletedQuest.tasksNeededForCompletion[t] == tasks[s].taskName)
                        {
                            if (!tasks[s].hasBeenCompleted)
                            {
                                allTasksCompleted = false;
                            }
                        }
                    }
                }

                if (allTasksCompleted)
                {
                    quests[i].hasBeenCompleted = true;
                } else
                {
                    print("no quests completed");
                }
            }
        }
    }

    public bool CheckQuestComplete(string questName)
    {
        bool questComplete = false;
        for (int i = 0; i < quests.Length; i++)
        {
            if(questName == quests[i].questName)
            {
                if (quests[i].hasBeenCompleted)
                {
                    questComplete = true;
                }
            }
        }

        return questComplete;
    }

    public void SetAllTasksComplete()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].hasBeenCompleted = true;
        }
    }

    public void ResetAllTasks()
    {
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].hasBeenCompleted = false;
        }
    }
}

[System.Serializable]
public class Task
{
    public string taskName;
    public bool hasBeenCompleted;
}

[System.Serializable]
public class Quest
{
    public string questName;
    public bool hasBeenCompleted;
    public string[] tasksNeededForCompletion;
}