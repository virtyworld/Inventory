
using System.Collections.Generic;

public class BackpackMenuModel 
{
    private List<ItemType> itemTypes = new List<ItemType>();

    public List<ItemType> ItemTypes => itemTypes;

    public void AddItem(ItemType itemType)
    {
        if (!itemTypes.Contains(itemType))itemTypes.Add(itemType);
    }
    public void DeleteItem(ItemType itemType)
    {
        itemTypes.Remove(itemType);
    }
}
