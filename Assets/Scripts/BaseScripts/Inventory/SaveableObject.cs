using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType {Pickup, Enemy, ItemThree }

public abstract class SaveableObject : MonoBehaviour
{
    protected string saveInfo;
    private InventoryManager inventoryManager;
    private ObjectType objectType;

    // Start is called before the first frame update
    private void Start()
    {
        //SaveGameManager.Instance.SaveableObjects.Add(this);
        inventoryManager = InventoryManager.Instance;

    }

    public virtual void Save(int id)
    {
        //Pickup_3.3, 7.2, 5.5_1
        inventoryManager.savedObjects[id] = objectType + "_" + transform.position.ToString() + "_" + saveInfo;
        
    }

    public virtual void Load(string[] values)
    {
        //transform.localPosition = SaveGameManager.Instance.StringToVector(values[1]);
    }

    public void DestroySaveable()
    {

    }

  
}
