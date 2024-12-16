using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Этот скрипт реализует контроллер рюкзака в Unity, который управляет добавлением объектов в рюкзак, их анимацией и удалением.
// 1. Инициализация: настроены связи между моделью, представлением и событиями для добавления/удаления объектов.
// 2. Обработка ввода: при удерживании мыши на объекте привязывает его к рюкзаку после заданного времени.
// 3. Привязка объектов: плавное перемещение и вращение объектов в рюкзак.
// 4. Удаление объектов: объекты могут быть удалены из рюкзака на основе их типа.
public class BackpackController : MonoBehaviour
{
    private BackpackModel model;
    private BackpackView view;
    private UnityEvent<GameObject> onObjectAttachedToBackpack;
    private List<ItemView> itemList = new List<ItemView>();
    private float mouseDownTime;
    private bool isMouseDown;
    private float snapThreshold = 0.3f;
    private bool isSnapped;

    public void Init(BackpackModel model, BackpackView view, List<ItemView> itemList,
        UnityEvent<GameObject> onObjectAttachedToBackpack, UnityEvent<ItemType> onObjectDetachedToBackpack)
    {
        this.model = model;
        this.view = view;
        this.itemList = itemList;
        this.onObjectAttachedToBackpack = onObjectAttachedToBackpack;
        onObjectDetachedToBackpack.AddListener(ObjectDetachedToBackpack);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownTime = Time.time; 
            isMouseDown = true;
            isSnapped = false; 
        }

        if (isMouseDown && !isSnapped)
        {
            float holdDuration = Time.time - mouseDownTime;

            if (holdDuration > snapThreshold)
            {
                SnapObjectsToBackpack(); 
                isSnapped = true; 
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false; 
        }
    }

    private void SnapObjectsToBackpack()
    {
        Collider[] hitColliders = view.GetObjectsInSelection();

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.transform.name.Equals("BackPack")) continue;

            ItemView item = hitCollider.GetComponent<ItemView>();

            if (item != null)
            {
                onObjectAttachedToBackpack?.Invoke(item.Rigidbody.gameObject);
                model.AddObjectToBackpack(item);
                StartCoroutine(SmoothMoveAndRotate(hitCollider.transform, item.ItemConfig.position,
                    item.ItemConfig.rotation, 0.5f));
            }
            else
            {
                Debug.Log($"{hitCollider.name} does not have an ItemView component.");
            }
        }
    }

    private IEnumerator SmoothMoveAndRotate(Transform objectTransform, Vector3 targetPosition,
        Quaternion targetRotation, float duration)
    {
        Vector3 startPosition = objectTransform.position;
        Quaternion startRotation = objectTransform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            t = Mathf.SmoothStep(0f, 1f, t);

            objectTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            objectTransform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        objectTransform.position = targetPosition;
        objectTransform.rotation = targetRotation;
    }

    private void ObjectDetachedToBackpack(ItemType type)
    {
        foreach (ItemView item in itemList)
        {
            if (item.ItemConfig.itemType == type) model.RemoveObjectFromBackpack(item);
        }
    }
}