using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [System.Serializable]
    public class ItemStack
    {
        public ItemsSOScript item;
        public int amount;

        public ItemStack(ItemsSOScript item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }
    }

    public List<ItemStack> hybrids = new();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // =========================
    // ADD HYBRID
    // =========================
    public void AddHybrid(ItemsSOScript hybrid)
    {
        if (hybrid == null) return;

        ItemStack stack = hybrids.Find(h => h.item == hybrid);

        if (stack != null)
        {
            stack.amount++;
        }
        else
        {
            hybrids.Add(new ItemStack(hybrid, 1));
        }

        Debug.Log($"Added {hybrid.name}. Total: {GetCount(hybrid)}");
    }

    // =========================
    // REMOVE HYBRID
    // =========================

    public void RemoveHybrid(ItemsSOScript hybrid)
    {
        ItemStack stack = hybrids.Find(h => h.item == hybrid);

        if (stack == null) return;

        stack.amount--;

        if (stack.amount <= 0)
            hybrids.Remove(stack);
    }

    // =========================
    // GET COUNT
    // =========================
    public int GetCount(ItemsSOScript hybrid)
    {
        ItemStack stack = hybrids.Find(h => h.item == hybrid);

        if (stack != null)
            return stack.amount;

        return 0;
    }

    public void Clear()
    {
        hybrids.Clear();
    }
}
