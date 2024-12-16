using UnityEngine;
using UnityEngine.Events;

public class BackpackView : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private LayerMask selectableLayer;
    
    private BackpackModel model;
    private bool isHovering;
    private bool isBackpackOpen;
    private UnityEvent onHideBackpackMenu;
    private UnityEvent onShowBackpackMenu;
    
    public void Init(UnityEvent onHideBackpackMenu,UnityEvent onShowBackpackMenu)
    {
        this.onHideBackpackMenu = onHideBackpackMenu;
        this.onShowBackpackMenu = onShowBackpackMenu;
    }
    private void Start()
    {
        if (!collider && TryGetComponent(out Collider backpackCollider))
        {
            collider = backpackCollider;
        }
    }
    private void Update()
    {
        if (isHovering && Input.GetMouseButton(0)&& !isBackpackOpen)
        {
            ShowBackpackContents();
        }
        else if (Input.GetMouseButtonUp(0) && isBackpackOpen) 
        {
            HideBackpackContents();
        }
    }
    public Collider[] GetObjectsInSelection()
    {
        if (collider)
        {
            Vector3 center = collider.bounds.center;
            Vector3 size = collider.bounds.size;
            size.z *= 2;
            return Physics.OverlapBox(center, size / 2f, Quaternion.identity, selectableLayer);
        }

        Debug.LogError("Backpack does not have a Collider component.");
        return new Collider[0];
    }
    private void OnMouseEnter()
    {
        isHovering = true;
    }

    private void OnMouseExit()
    {
        isHovering = false;
        
        if (isBackpackOpen)  HideBackpackContents();
    }

    private void ShowBackpackContents()
    {
        isBackpackOpen = true;
        onShowBackpackMenu?.Invoke();
    }

    private void HideBackpackContents()
    {
        isBackpackOpen = false;
        onHideBackpackMenu?.Invoke();
    }
}