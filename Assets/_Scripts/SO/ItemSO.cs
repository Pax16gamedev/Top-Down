using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public float damage;
    public Sprite icon;
}
