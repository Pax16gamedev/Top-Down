using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI itemNameTMP;
    [SerializeField] TextMeshProUGUI itemCountTMP;

    private int itemCount;

    public void FeedData(ItemSO item)
    {
        icon.sprite = item.icon;
        itemNameTMP.text = item.itemName;
        UpdateStackInfo();
    }

    public void UpdateStackInfo()
    {
        itemCount++;
        itemCountTMP.text = $"x{itemCount}";
    }
}
