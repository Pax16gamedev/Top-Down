using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemInfo : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemNameTMP;
    [SerializeField] TextMeshProUGUI itemCountTMP;

    private ItemSO currentItemData;
    private int itemCount;

    [Header("Drag settings")]
    private float onDragAlpha = 0.5f;

    private Canvas canvas; // Componente canvas (dentro del GO Canvas)
    private CanvasGroup canvasGroup;
    private RectTransform itemRectTransform;

    private Transform initParent; // Slot original al cual estoy anclado
    private Vector3 initPosition; // Posicion original en la que nazco

    public Transform InitParent => initParent;
    public Vector3 InitPosition => initPosition;

    private void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        itemRectTransform = GetComponent<RectTransform>();
    }

    public void FeedData(ItemSO item)
    {
        currentItemData = item;
        icon.sprite = item.icon;
        itemNameTMP.text = item.itemName;
        UpdateStackInfo();
    }

    public void UpdateStackInfo()
    {
        itemCount++;
        itemCountTMP.text = $"x{itemCount}";
    }

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

        // Dropeo fallido
        if(itemRectTransform.parent == canvas.transform)
        {
            // Vuelvo a mi posicion original
            itemRectTransform.SetParent(initParent);
            itemRectTransform.localPosition = initPosition;
        }
    }

    public void UseItem()
    {
        if(currentItemData == null) return;

        print($"Utilizo {currentItemData.itemName}");
    }
}
