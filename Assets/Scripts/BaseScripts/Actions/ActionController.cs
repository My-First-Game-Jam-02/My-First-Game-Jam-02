using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class ActionController : MonoBehaviour
{
    private SingleAction[] actions;
    private SingleAction activeAction;
    private ActionSequence currentActionSequenceLoaded;

    [HideInInspector]
    public SSPlayerController playerController;
    
    public CinemachineVirtualCamera currentCamera;

    public ActionSequence[] actionSequences;

    void Awake()
    {
        actions = GetComponentsInChildren<SingleAction>();
        playerController = FindObjectOfType<SSPlayerController>();
        currentCamera = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        actionSequences = GetComponentsInChildren<ActionSequence>();

        DeactivateAllActions();
    }

    private void Start()
    {
        ActivateCorrectActionSequence();
        ActivateFirstAction();
    }

    public void DeactivateAllActions()
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].gameObject.SetActive(false);
        }
    }

    public void ActivateCorrectActionSequence()
    {
        
        foreach(ActionSequence actSeq in actionSequences)
        {
            bool isTrue = true;

            if(actSeq.taskToCheck.Length >= 0)
            {
                foreach (Task expectedTask in actSeq.taskToCheck)
                {

                    if (!QuestManager.Instance.CheckTaskComplete(expectedTask.taskName))
                    {
                        isTrue = false;
                    }
                }
            }

            if (isTrue)
            {
                if(currentActionSequenceLoaded == null)
                {
                    currentActionSequenceLoaded = actSeq;
                }
                else
                {
                    if (currentActionSequenceLoaded.priority < actSeq.priority)
                    {
                        currentActionSequenceLoaded = actSeq;
                    }
                }
            }
        }

        for (int i = 0; i < actionSequences.Length; i++)
        {
            if(actionSequences[i] != currentActionSequenceLoaded)
            {
                actionSequences[i].gameObject.SetActive(false);
            }
        }
    }

    public void ActivateFirstAction()
    {
        if (currentActionSequenceLoaded == null)
        {
            currentActionSequenceLoaded = actionSequences[0];
            
        }
        activeAction = currentActionSequenceLoaded.startingAction;
        activeAction.gameObject.SetActive(true);
    }

    public SingleAction GetActiveAction()
    {
        return activeAction;
    }
}


