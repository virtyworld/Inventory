using UnityEngine;

public class ItemView : MonoBehaviour
{
  [SerializeField] private ItemConfig itemConfig;
  [SerializeField] private Rigidbody rigidbody;

  public ItemConfig ItemConfig => itemConfig;
  public Rigidbody Rigidbody => rigidbody;

  public void FreezeRigidbody()
  {
    rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
  }
  public void UnSnapObject()
  {
    UnFreezeRigidbody();
    AddForceInDirection(Vector3.back, 2f);
  }
  public void UnFreezeRigidbody()
  {
    rigidbody.constraints =RigidbodyConstraints.None;
  }
  private void AddForceInDirection(Vector3 direction, float magnitude)
  {
    Vector3 force = direction.normalized * magnitude;
    AddForce(force);
  }
  private void AddForce(Vector3 force)
  {
    if (!rigidbody.isKinematic)
    {
      rigidbody.AddForce(force, ForceMode.Impulse); 
    }
  }
  
}
