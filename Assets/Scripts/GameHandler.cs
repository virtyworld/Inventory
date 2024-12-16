using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// Этот скрипт управляет игровыми объектами в Unity, включая объекты для перетаскивания, рюкзак и меню рюкзака.
// 1. Инициализация: настраиваются связи между различными моделями, представлениями и контроллерами для перетаскивания, рюкзака, предметов и меню рюкзака.
// 2. Обработка перетаскивания: контроллер перетаскивания инициализирует и управляет объектами, которые могут быть перетащены.
// 3. Управление рюкзаком: добавление и удаление предметов в/из рюкзака с учетом событий привязки и отвязки.
// 4. Меню рюкзака: управление видимостью меню рюкзака и отправка запросов на сервер с событиями привязки/отвязки объектов.
//  Общение между разными контроллерами происходит через UnityEvent. между контроллером и моделью вьюшкой не успел сделать,увы. Сделал обычные паблик методы
public class GameHandler : MonoBehaviour
{
    [Header("DragAndDrop")]
    [SerializeField] private List<DragAndDropView> dragAndDropViewItemList;
    [SerializeField] private DragAndDropController dragAndDropController;
    [Header("Backpack")]
    [SerializeField] private BackpackView backpackView;
    [SerializeField] private BackpackController backpackController;
    [Header("Item")]
    [SerializeField] private List<ItemView> itemList;
    [SerializeField] private ItemController itemController;
    [Header("BackpackMenu")]
    [SerializeField] private BackpackMenuView backpackMenuView;
    [SerializeField] private BackpackMenuController backpackMenuController;
    
    
    private List<DragAndDropModel> dragAndDropModelList = new List<DragAndDropModel>();
    private List<ItemModel> itemModelList = new List<ItemModel>();
    private BackpackModel backpackModel;
    private BackpackMenuModel backpackMenuModel;
    private UnityEvent<GameObject> onObjectAttachedToBackpack = new UnityEvent<GameObject>();
    private UnityEvent<ItemType> onObjectDetachedToBackpack = new UnityEvent<ItemType>();
    private UnityEvent<GameObject> onAddItemToBackpack = new UnityEvent<GameObject>();
    private UnityEvent onHideBackpackMenu = new UnityEvent();
    private UnityEvent onShowBackpackMenu = new UnityEvent();
    private string url = "https://wadahub.manerai.com/api/inventory/status";
    private string bearerToken = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";
    
    void Start()
    {
        onObjectAttachedToBackpack.AddListener(ObjectAttachedToBackpack);
        onObjectDetachedToBackpack.AddListener(ObjectDetachedToBackpack);
        InitItem();
        InitBackpack();
        InitDragAndDrops();
        InitBackpackMenu();
    }

    private void InitDragAndDrops()
    {
        foreach (var dadi in dragAndDropViewItemList)
        {
            dragAndDropModelList.Add(new DragAndDropModel());  
        }
        dragAndDropController.Init(dragAndDropModelList,dragAndDropViewItemList); 
    }

    private void InitBackpack()
    {
        backpackView.Init(onHideBackpackMenu,onShowBackpackMenu);
        backpackModel = new BackpackModel();
        backpackController.Init(backpackModel,backpackView,itemList,onObjectAttachedToBackpack,onObjectDetachedToBackpack);
    }
    private void InitItem()
    {
        foreach (var value in itemList)
        {
            itemModelList.Add(new ItemModel());  
        }
        itemController.Init(itemModelList,itemList,onAddItemToBackpack); 
    }

    private void InitBackpackMenu()
    {
        backpackMenuModel = new BackpackMenuModel();
        backpackMenuView.Init(backpackMenuModel,onHideBackpackMenu,onShowBackpackMenu,onObjectDetachedToBackpack);
        backpackMenuController.Init(backpackMenuModel,backpackMenuView,itemList,onAddItemToBackpack);
    }
    private void ObjectAttachedToBackpack(GameObject gm)
    {
        foreach (ItemView item in itemList)
        {
            if (item.gameObject==gm)
            {
               StartCoroutine(SendPostRequest("attached " + item.ItemConfig.itemType));
            }
        }
        
        onAddItemToBackpack?.Invoke(gm);
    }
    private void ObjectDetachedToBackpack(ItemType type)
    {
        StartCoroutine(SendPostRequest("detached " + type));
        
        foreach (ItemView item in itemList)
        {
            if (item.ItemConfig.itemType==type)
            {
                item.UnSnapObject();
            }
        }
    }
    
    private IEnumerator SendPostRequest(string eventString)
    {
        RequestData requestData = new RequestData();
        requestData.itemId = 1;
        requestData.eventName = eventString;
        string jsonData = JsonUtility.ToJson(requestData);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {bearerToken}");
        
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Response: {request.downloadHandler.text}");
        }
        else
        {
            Debug.LogError($"Error: {request.error}");
        }
    }
    
}
