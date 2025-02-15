using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [SerializeField] GameObject inventoryPanel;
    [SerializeField] ItemSlot[] slotsInventario;

    [SerializeField] ItemSlot[] quickSlots;

    private ItemInfo[] itemInfo;
    private List<ItemSO> itemsInventario = new List<ItemSO>();


    private int itemsCollected;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitSlots();
    }

    private void InitSlots()
    {
        itemInfo = new ItemInfo[slotsInventario.Length];
        for(int i = 0; i < slotsInventario.Length; i++)
        {
            itemInfo[i] = slotsInventario[i].GetComponentInChildren<ItemInfo>();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            quickSlots[0].GetComponentInChildren<ItemInfo>().UseItem();
        }
    }

    public void AddNewItem(ItemSO item)
    {
        if(itemsInventario.Contains(item))
        {
            // Actualizo
            int indexOfStackItem = itemsInventario.IndexOf(item);
            itemInfo[indexOfStackItem].UpdateStackInfo();
        }
        else
        {
            itemsInventario.Add(item);
            slotsInventario[itemsCollected].gameObject.SetActive(true);

            itemInfo[itemsCollected].FeedData(item);

            itemsCollected++;
        }
    }

    public void UseSelectedItem(ItemSO item)
    {
        //TODO
    }
}
