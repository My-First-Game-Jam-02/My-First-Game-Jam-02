using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : SaveableObject
{
    public string itemName;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Save(int id)
    {
        saveInfo = itemName;
        base.Save(id);
    }

    public override void Load(string[] values)
    {
        //itemName = int.Parse(values[2]);

        base.Load(values);

    }
}
