using UnityEngine;

[CreateAssetMenu(fileName = "ItemType", menuName = "Config/ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
  public int weight;
  public int name;
  public int id;
  public Vector3 position ;
  public Quaternion rotation;
  public ItemType itemType;
}
