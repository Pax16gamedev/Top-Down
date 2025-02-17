using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;

    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject hudPanel;

    [SerializeField] GridLayoutGroup inventoryGrid;
    [SerializeField] GridLayoutGroup inventoryQuickSlotGrid;
    [SerializeField] GridLayoutGroup hudQuickSlotGroup;

    [SerializeField] GameObject inventorySlotPrefab;
    [SerializeField] GameObject inventoryQuickSlotPrefab;
    [SerializeField] GameObject hudQuickSlotPrefab;

    [SerializeField] int selectedQuickSlot;
    [SerializeField] int inventorySize;
    [SerializeField] int quickSlotSize;

    ItemSlot[] inventorySlots;
    ItemSlot[] inventoryQuickSlots;
    ItemSlot[] hudQuickSlots;

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
        // Enable inventory to ensure Awake executes
        inventoryPanel.SetActive(true);
        // Clean up existing children in the grids
        foreach(var grid in new[] { inventoryGrid, inventoryQuickSlotGrid, hudQuickSlotGroup })
            foreach(Transform child in grid.transform)
                Destroy(child.gameObject);

        inventorySlots = new ItemSlot[inventorySize];
        inventoryQuickSlots = new ItemSlot[quickSlotSize];
        hudQuickSlots = new ItemSlot[quickSlotSize];

        // Create inventory slots
        for(int i = 0; i < inventorySize; i++)
        {
            inventorySlots[i] = Instantiate(inventorySlotPrefab, inventoryGrid.transform).GetComponent<ItemSlot>();
        }

        // Create quick slots
        for(int i = 0; i < quickSlotSize; i++)
        {
            inventoryQuickSlots[i] = Instantiate(hudQuickSlotPrefab, inventoryQuickSlotGrid.transform).GetComponent<ItemSlot>();
            inventoryQuickSlots[i].QuickSlotText = (i + 1).ToString();
            inventoryQuickSlots[i].Selected = i == 0;

            hudQuickSlots[i] = Instantiate(hudQuickSlotPrefab, hudQuickSlotGroup.transform).GetComponent<ItemSlot>();
            hudQuickSlots[i].SyncSlot = inventoryQuickSlots[i];
        }

        itemInfo = new ItemInfo[inventorySlots.Length];
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            itemInfo[i] = inventorySlots[i].GetComponentInChildren<ItemInfo>();
        }
        inventoryPanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            hudPanel.SetActive(!inventoryPanel.activeSelf);
        }

        for(int i = 0; i < quickSlotSize; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                inventoryQuickSlots[selectedQuickSlot].Selected = false;
                selectedQuickSlot = i;
                inventoryQuickSlots[selectedQuickSlot].Selected = true;
            }
        }
    }

    public void AddNewItem(ItemSO item)
    {
        if(itemsInventario.Contains(item))
        {
            // Actualizo
            int indexOfStackItem = itemsInventario.IndexOf(item);
            itemInfo[indexOfStackItem].Stack.Add();
        }
        else
        {
            itemsInventario.Add(item);
            inventorySlots[itemsCollected].gameObject.SetActive(true);

            itemInfo[itemsCollected].Stack = item;

            itemsCollected++;
        }
    }

    public ItemStack PeekItem()
    {
        var stack = inventoryQuickSlots[selectedQuickSlot].ItemInfo.Stack;
        return stack ? stack : null;
    }

    public (ItemSO Item, int Count) UseItem(int count = 1)
    {
        var stack = inventoryQuickSlots[selectedQuickSlot].ItemInfo.Stack;
        var used = stack ? stack.Use(count) : 0;
        return used > 0 ? (stack.Item, used) : (null, 0);
    }
}
