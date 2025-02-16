using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using static UnityEditor.Progress;

public class ItemInfo : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemNameTMP;
    [SerializeField] TextMeshProUGUI itemCountTMP;
    [SerializeField] Sprite transparentSprite;

    private ItemStack stack;
    private ItemInfo synchInfo;

    public ItemSlot Slot { get; private set; }

    public event Action<ItemStack> OnStackUpdate;

    public ItemInfo SynchInfo
    {
        get => synchInfo;
        set
        {
            if(synchInfo == value)
                return;

            if(synchInfo != null)
                synchInfo.OnStackUpdate -= OnStackUpdated;
            synchInfo = value;
            if(synchInfo != null)
                synchInfo.OnStackUpdate += OnStackUpdated;

            OnStackUpdated(value.Stack);
        }
    }

    public ItemStack Stack
    {
        get => stack;
        set
        {
            if(stack == value)
                return;

            if(stack != null)
                stack.OnUpdate -= OnStackUpdated;
            stack = value;
            if(stack != null)
                stack.OnUpdate += OnStackUpdated;

            OnStackUpdated(value);
        }
    }

    private void OnStackUpdated(ItemStack stack)
    {
        if(stack?.Item)
        {
            icon.sprite = stack.Item.Icon;
            itemNameTMP.text = stack.Item.ItemName;
            itemCountTMP.text = stack.Item.Stackable ? $"x{stack.Count}" : null;
        }
        else
        {
            icon.sprite = transparentSprite;
            itemNameTMP.text = null;
            itemCountTMP.text = null;
        }
        OnStackUpdate?.Invoke(stack);
    }

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemRectTransform = GetComponent<RectTransform>();
        Slot = GetComponentInParent<ItemSlot>(true);
        OnStackUpdated(stack);
    }

    #region Drag&Drop

    [Header("Drag settings")]
    private float onDragAlpha = 0.5f;

    private Canvas canvas; // Componente canvas (dentro del GO Canvas)
    private CanvasGroup canvasGroup;
    private RectTransform itemRectTransform;

    private Transform initParent; // Slot original al cual estoy anclado
    private Vector3 initPosition; // Posicion original en la que nazco

    public Transform InitParent => initParent;
    public Vector3 InitPosition => initPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        initParent = itemRectTransform.parent;
        initPosition = itemRectTransform.localPosition;

        // Set Parent temporal
        itemRectTransform.SetParent(canvas.transform);

        canvasGroup.alpha = onDragAlpha; // Efecto transparencia
        canvasGroup.blocksRaycasts = false; // Evito lecturas
    }

    public void OnDrag(PointerEventData eventData)
    {
        itemRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        // Vuelvo a mi posicion original
        itemRectTransform.SetParent(initParent);
        itemRectTransform.localPosition = initPosition;
    }

    #endregion
}
