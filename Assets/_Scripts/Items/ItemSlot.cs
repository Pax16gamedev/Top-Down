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

    private ItemSlot syncSlot;
    private ItemSlot pairedSlot;
    private event Action<ItemSlot> OnSlotUpdate;

    public ItemInfo ItemInfo { get; private set; }
    public ItemSlot PairedSlot
    {
        get => pairedSlot;
        private set
        {
            if(pairedSlot == value)
                return;
            var prevSlot = pairedSlot;
            pairedSlot = value;
            if(prevSlot)
                prevSlot.PairedSlot = null;
            pairedSlot = value;
            if(pairedSlot)
                pairedSlot.PairedSlot = this;

            if(isQuickSlot)
                ItemInfo.Stack = pairedSlot ? pairedSlot.ItemInfo.Stack : null;
        }
    }

    public ItemSlot SyncSlot
    {
        get => syncSlot;
        set
        {
            if(syncSlot == value)
                return;

            if(syncSlot != null)
                syncSlot.OnSlotUpdate -= OnSlotUpdated;
            syncSlot = value;
            if(syncSlot != null)
                syncSlot.OnSlotUpdate += OnSlotUpdated;

            ItemInfo.SynchInfo = syncSlot.ItemInfo;

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
        if(draggedItemTransform.TryGetComponent(out ItemInfo draggedItemInfo))
        {
            if(draggedItemInfo.Slot.isQuickSlot == isQuickSlot)
            {
                // If both slots are quickslots, or both slots are inventory slots, and target has something, just swap values and quickslot references
                if(!isQuickSlot)
                    (ItemInfo.Stack, draggedItemInfo.Stack) = (draggedItemInfo.Stack, ItemInfo.Stack);

                (PairedSlot, draggedItemInfo.Slot.PairedSlot) = (draggedItemInfo.Slot.PairedSlot, PairedSlot);
            }
            else if(isQuickSlot)
            {
                // If this is a quickslot, and dragged is inventory, pair quickslot
                PairedSlot = draggedItemInfo.Slot;
            }
            // If this is an inventory slot, and dragged is quickslot, ignore
        }
    }
}
