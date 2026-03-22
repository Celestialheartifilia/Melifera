using UnityEngine;

public class OrderViewer : MonoBehaviour
{
    public GameObject orderPanel;
    public OrderBubbleUI orderBubbleUI;

    bool isOpen = false;

    void Start()
    {
        //if (orderPanel != null)
        //    orderPanel.SetActive(false);
    }

    public void ToggleOrderView()
    {
        if (OrderTakingManager.Instance == null)
        {
            Debug.LogError("OrderTakingManager not found.");
            return;
        }

        Debug.Log("Current Order: " + OrderTakingManager.Instance.currentOrder);

        if (OrderTakingManager.Instance.currentOrder == null)
        {
            Debug.LogWarning("No active order.");
            return;
        }

        isOpen = !isOpen;

        orderPanel.SetActive(isOpen);
        // Instead of SetActive
        //SpriteRenderer sr = orderPanel.GetComponent<SpriteRenderer>();

        //if (sr != null)
        //    sr.enabled = isOpen;

        //SpriteRenderer[] renderers = orderPanel.GetComponentsInChildren<SpriteRenderer>();

        //foreach (var r in renderers)
        //{
        //    r.enabled = isOpen;
        //}

        if (isOpen)
        {
            orderBubbleUI.DisplayOrder(OrderTakingManager.Instance.currentOrder);
        }
    }
}
