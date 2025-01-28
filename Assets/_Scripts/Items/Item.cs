using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] protected ItemSO item;

    public virtual void Interact()
    {
        GameManagerSO.Instance.InventorySystem.AddNewItem(item);
        Destroy(gameObject);
    }
}
