using System.Collections.Generic;
using UnityEngine;

public class BackpackModel
{
    public List<ItemView>  ObjectsInBackpack { get; private set; }
    
    public BackpackModel()
    {
        ObjectsInBackpack = new List<ItemView>();
    }
    
    public void AddObjectToBackpack(ItemView newItem)
    {
        ObjectsInBackpack.Add(newItem);
    }

    public void RemoveObjectFromBackpack(ItemView obj)
    {
        ObjectsInBackpack.RemoveAll(item => item == obj);
    }
}