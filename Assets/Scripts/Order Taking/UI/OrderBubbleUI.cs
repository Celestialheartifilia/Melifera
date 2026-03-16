using UnityEngine;

public class OrderBubbleUI : MonoBehaviour
{
    public SpriteRenderer[] itemSlots;

    public void DisplayOrder(OrderList order)
    {
        // Clear all slots first
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].sprite = null;
            itemSlots[i].gameObject.SetActive(false);

            // Reset scale
            itemSlots[i].transform.localScale = Vector3.one;
        }

        // Fill slots based on order items
        for (int i = 0; i < order.orderedItems.Count && i < itemSlots.Length; i++)
        {
            var item = order.orderedItems[i];

            itemSlots[i].sprite = item.itemSprite;
            itemSlots[i].gameObject.SetActive(true);

            // If item is flower, scale it up
            if (item.itemType == ItemType.Flower)
            {
                itemSlots[i].transform.localScale = Vector3.one * 1.5f;
            }
        }
    }
}
