using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionSequence: MonoBehaviour
{
    public SingleAction startingAction;
    public Task[] taskToCheck;
    public int priority;

    public ActionSequence()
    {

    }
   
}
