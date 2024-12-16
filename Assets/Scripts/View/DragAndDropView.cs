using UnityEngine;

public class DragAndDropView : MonoBehaviour
{
    public void UpdatePosition(Vector3 position)
    {
        transform.position = position;
    }

    public void Highlight(bool highlight)
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = highlight ? Color.yellow : Color.white;
        }
    }
    
    
}