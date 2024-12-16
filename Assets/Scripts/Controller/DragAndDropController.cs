using System.Collections.Generic;
using UnityEngine;

// Этот скрипт управляет логикой перетаскивания объектов в Unity, позволяя пользователю взаимодействовать с объектами через мышь.
// 1. Инициализация: подключает модели и представления для объектов, которые могут быть перетащены.
// 2. Выбор объекта: при нажатии на объект мышью происходит его выбор и начало перетаскивания.
// 3. Перетаскивание: когда объект выбран, его положение обновляется в зависимости от положения мыши.
// 4. Отмена выбора: при отпускании кнопки мыши объект перестает перетаскиваться, и его выделение снимается.
public class DragAndDropController : MonoBehaviour
{
    private Camera mainCamera;
    private DragAndDropModel currentModel;
    private DragAndDropView currentView;
    private List<DragAndDropModel> modelList = new List<DragAndDropModel>();
    private List<DragAndDropView> viewList = new List<DragAndDropView>();
    
    public void Init( List<DragAndDropModel> modelList, List<DragAndDropView> viewList)
    {
        this.modelList = modelList;
        this.viewList = viewList;
        mainCamera = Camera.main;
    }
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectObject();
        }
        else if (Input.GetMouseButton(0) && currentModel != null && currentModel.IsDragging)
        {
            DragSelectedObject();
        }
        else if (Input.GetMouseButtonUp(0) && currentModel != null)
        {
            DeselectObject();
        }
    }

    private void TrySelectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            for (int i = 0; i < viewList.Count; i++)
            {
                if (viewList[i].gameObject == hit.collider.gameObject)
                {
                    currentView = viewList[i];
                    currentModel = modelList[i];

                    if (currentModel != null && currentView != null)
                    {
                        float zCoordinate = mainCamera.WorldToScreenPoint(hit.transform.position).z;
                        Vector3 offset = hit.transform.position - GetMouseWorldPosition(zCoordinate);
                        currentModel.StartDragging(offset, zCoordinate);
                        currentView.Highlight(true);
                        //TODO event
                        ItemView item = hit.collider.GetComponent<ItemView>();
                        if (item != null)item.UnFreezeRigidbody();
                        break;
                    }
                }
            }
        }
    }

    
    private void DragSelectedObject()
    {
        Vector3 newPosition = GetMouseWorldPosition(currentModel.ZCoordinate) + currentModel.Offset;
        currentView.UpdatePosition(newPosition);
    }

    private void DeselectObject()
    {
        currentModel.StopDragging();
        currentView.Highlight(false);

        currentModel = null;
        currentView = null;
    }

    private Vector3 GetMouseWorldPosition(float zCoordinate)
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = zCoordinate;
        return mainCamera.ScreenToWorldPoint(mouseScreenPosition);
    }
}
