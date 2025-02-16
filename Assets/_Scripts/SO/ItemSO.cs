using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] float damage;
    [SerializeField] bool stackable;
    [SerializeField] bool removeOnUse;
    [SerializeField] Sprite icon;

    public string ItemName => itemName;
    public float Damage => damage;
    public bool Stackable => stackable;
    public bool RemoveOnUse => removeOnUse;
    public Sprite Icon => icon;
}

[Serializable]
public class ItemStack
{
    public ItemStack(ItemSO item, int count = 1)
    {
        Item = item;
        Count = count;
    }

    public static implicit operator ItemStack(ItemSO item) => new(item);

    public static implicit operator bool(ItemStack item) => item != null && item.CanUse();

    public ItemSO Item { get; }

    public int Count { get; private set; }

    public bool CanUse()
        => Item != null && Count > 0;

    public int Use(int useCount = 1)
    {
        if(useCount <= 0)
            throw new ArgumentException(nameof(useCount));
        if(Item.RemoveOnUse)
        {
            var previous = Count;
            Count -= useCount;
            if(Count < 0)
                Count = 0;
            OnUpdate?.Invoke(this);
            return previous - Count;
        }
        else
        {
            return Count;
        }
    }

    public void Add(int useCount = 1)
    {
        if(useCount <= 0)
            throw new ArgumentException(nameof(useCount));

        Count += useCount;

        if(Count > 1 && !Item.Stackable)
            Count = 1;

        OnUpdate?.Invoke(this);
    }

    public event Action<ItemStack> OnUpdate;
}
