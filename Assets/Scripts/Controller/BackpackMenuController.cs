using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackpackMenuController : MonoBehaviour
{
    private BackpackMenuModel model;
    private BackpackMenuView view;
    private List<ItemView> itemViewList;
    
    public void Init(BackpackMenuModel model,BackpackMenuView view,List<ItemView> viewList,UnityEvent<GameObject> onAddItemToBackpack)
    {
        this.model = model;
        this.view = view;
        itemViewList = viewList;
        onAddItemToBackpack.AddListener(AddItemToBackpackMenu);
    }
    
    private void AddItemToBackpackMenu(GameObject go)
    {
        foreach (ItemView item in itemViewList)
        {
            if (item.gameObject==go)
            {
                model.AddItem(item.ItemConfig.itemType);
                view.UpdateMenu();
            }
        }
    }
}
