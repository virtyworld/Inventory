using UnityEngine;

public class DragAndDropModel 
{
    public Vector3 Offset { get; private set; }
    public float ZCoordinate { get; private set; }
    public bool IsDragging { get; private set; }

    public void StartDragging(Vector3 offset, float zCoordinate)
    {
        Offset = offset;
        ZCoordinate = zCoordinate;
        IsDragging = true;
    }

    public void StopDragging()
    {
        Offset = Vector3.zero;
        ZCoordinate = 0f;
        IsDragging = false;
    }
}