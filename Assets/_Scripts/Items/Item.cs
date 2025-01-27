using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO item;

    public void Interact()
    {
        GameManagerSO.Instance.InventorySystem.NewItem(item);
        Destroy(gameObject);
    }
}
