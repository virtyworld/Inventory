using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemController : MonoBehaviour
{
    private List<ItemModel> modelList;
    private List<ItemView> viewList;

    public void Init(List<ItemModel> modelList,List<ItemView> viewList,UnityEvent<GameObject> onFreezeRigidbodyToGameObject)
    {
        this.modelList = modelList;
        this.viewList = viewList;
        onFreezeRigidbodyToGameObject.AddListener(FreezeRigidbody);
    }

    private void FreezeRigidbody(GameObject go)
    {
        foreach (ItemView item in viewList)
        {
            if (item.gameObject==go)
            {
                item.FreezeRigidbody();
            }
        }
    }
}
