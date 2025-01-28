using UnityEngine;
using UnityEngine.EventSystems;

// Clase que se encarga de recoger el objeto arrastrado del inventario
public class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] RectTransform itemBackground;

    private ItemInfo currentItemInfo;

    private void Awake()
    {
        currentItemInfo = GetComponentInChildren<ItemInfo>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Transform draggedItemTransform = eventData.pointerDrag.transform;
        ItemInfo draggedItemInfo = draggedItemTransform.GetComponent<ItemInfo>();

        if(itemBackground.childCount > 0) // Ya hay datos en este slot, genero un intercambio
        {
            currentItemInfo.transform.SetParent(draggedItemInfo.InitParent);
            currentItemInfo.transform.localPosition = Vector3.zero;

            // Actualizo slot destino
            draggedItemInfo.InitParent.GetComponentInParent<ItemSlot>().currentItemInfo = currentItemInfo;
        }

        draggedItemTransform.SetParent(itemBackground);
        draggedItemTransform.localPosition = Vector3.zero; // Centrado al slot

        currentItemInfo = draggedItemInfo;
    }
}
