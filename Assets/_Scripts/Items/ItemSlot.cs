using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Clase que se encarga de recoger el objeto arrastrado del inventario
public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] TextMeshProUGUI quickSlotText;
    [SerializeField] RectTransform selectedBackground;
    [SerializeField] bool isQuickSlot;

    private ItemSlot synchSlot;
    private event Action<ItemSlot> OnSlotUpdate;

    public ItemInfo ItemInfo { get; private set; }
    public ItemSlot QuickSlot { get; private set; }
    public ItemSlot SynchSlot
    {
        get => synchSlot;
        set
        {
            if(synchSlot == value)
                return;

            if(synchSlot != null)
                synchSlot.OnSlotUpdate -= OnSlotUpdated;
            synchSlot = value;
            if(synchSlot != null)
                synchSlot.OnSlotUpdate += OnSlotUpdated;

            ItemInfo.SynchInfo = synchSlot.ItemInfo;

            OnSlotUpdated(value);
        }
    }

    private void OnSlotUpdated(ItemSlot value)
    {
        if(value)
        {
            QuickSlotText = value.QuickSlotText;
            Selected = value.Selected;
        }
        else
        {
            QuickSlotText = null;
            Selected = false;
        }
    }

    public string QuickSlotText { set { if(quickSlotText && value != QuickSlotText) { quickSlotText.text = value; OnSlotUpdate?.Invoke(this); } } get => quickSlotText.text; }
    public bool Selected { set { if(selectedBackground && value != Selected) { selectedBackground.gameObject.SetActive(value); OnSlotUpdate?.Invoke(this); } } get => selectedBackground.gameObject.activeSelf; }

    private void Awake()
    {
        ItemInfo = GetComponentInChildren<ItemInfo>(true);
        QuickSlotText = null;
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        Transform draggedItemTransform = eventData.pointerDrag.transform;
        ItemInfo draggedItemInfo = draggedItemTransform.GetComponent<ItemInfo>();

        if(draggedItemInfo.Slot.isQuickSlot == isQuickSlot) 
        {
            // If both slotas are quickslots, or both slots are inventory slots, just swap values and quickslot references
            (ItemInfo.Stack, draggedItemInfo.Stack) = (draggedItemInfo.Stack, ItemInfo.Stack);
            (ItemInfo.Slot.QuickSlot, draggedItemInfo.Slot.QuickSlot) = (draggedItemInfo.Slot.QuickSlot, ItemInfo.Slot.QuickSlot);
        }
        else if(isQuickSlot)
        {
            // If this is a quickslot, and dragged is inventory, remove the original quickslot if any and reasign to this one
            if(draggedItemInfo.Slot.QuickSlot)
                draggedItemInfo.Slot.QuickSlot.ItemInfo.Stack = null;
            ItemInfo.Stack = draggedItemInfo.Stack;
            draggedItemInfo.Slot.QuickSlot = this;
        }
        // If this is an inventory slot, and dragged is quickslot, ignore
    }
}
