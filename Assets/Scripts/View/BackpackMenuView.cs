using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackpackMenuView : MonoBehaviour
{
    [SerializeField] private GameObject mainFolder;
    [SerializeField] private Button sphereButton;
    [SerializeField] private Button cylinderButton;
    [SerializeField] private Button capsuleButton;

    private BackpackMenuModel model;
    private Dictionary<ItemType, Button> itemTypeToButtonMap;
    private EventTrigger sphereEventTrigger;
    private EventTrigger cylinderEventTrigger;
    private EventTrigger capsuleEventTrigger;
    private UnityEvent<ItemType> onObjectDetachedToBackpack = new UnityEvent<ItemType>();
    
    public void Init(BackpackMenuModel model, UnityEvent onHideBackpackMenu, UnityEvent onShowBackpackMenu,UnityEvent<ItemType> onObjectDetachedToBackpack)
    {
        this.model = model;
        onHideBackpackMenu.AddListener(HideMenu);
        onShowBackpackMenu.AddListener(ShowMenu);
        this.onObjectDetachedToBackpack = onObjectDetachedToBackpack;
    }

    private void Awake()
    {
        itemTypeToButtonMap = new Dictionary<ItemType, Button>
        {
            { ItemType.Sphere, sphereButton },
            { ItemType.Cylinder, cylinderButton },
            { ItemType.Capsule, capsuleButton }
        };
    }

    private void Start()
    {
        DisableAllButtons();
        HideMenu();
    }
    
    public void OnPointerUpSphereClicked()
    {
        onObjectDetachedToBackpack?.Invoke(ItemType.Sphere);
        model.DeleteItem(ItemType.Sphere);
        UpdateMenu();
    }
    public void OnPointerUpCylinderClicked()
    {
        onObjectDetachedToBackpack?.Invoke(ItemType.Cylinder);
        model.DeleteItem(ItemType.Cylinder);
        UpdateMenu();
    }
    public void OnPointerUpCapsuleClicked()
    {
        onObjectDetachedToBackpack?.Invoke(ItemType.Capsule);
        model.DeleteItem(ItemType.Capsule);
        UpdateMenu();
    }
    private void HideMenu()
    {
        mainFolder.SetActive(false);
    }

    private void ShowMenu()
    {
        mainFolder.SetActive(true);
    }

    public void UpdateMenu()
    {
        DisableAllButtons();

        foreach (ItemType type in model.ItemTypes)
        {
            if (itemTypeToButtonMap.TryGetValue(type, out Button button))
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    private void DisableAllButtons()
    {
        foreach (var button in itemTypeToButtonMap.Values)
        {
            button.gameObject.SetActive(false);
        }
    }
}