using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] ItemSlot[] slots;
    private ItemInfo[] itemInfo;

    private List<ItemSO> items = new List<ItemSO>();


    private int itemsCollected;

    private void Awake()
    {
        itemInfo = new ItemInfo[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            itemInfo[i] = slots[i].GetComponentInChildren<ItemInfo>();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    public void NewItem(ItemSO item)
    {
        items.Add(item);
        slots[itemsCollected].gameObject.SetActive(true);

        itemInfo[itemsCollected].FeedData(item);

        itemsCollected++;
    }
}
