using UnityEngine;

public class OrderViewer : MonoBehaviour
{
    public GameObject orderPanel;
    public OrderBubbleUI orderBubbleUI;

    bool isOpen = false;

    void Start()
    {
        if (orderPanel != null)
            orderPanel.SetActive(false);
    }

    public void ToggleOrderView()
    {
        if (OrderTakingManager.Instance == null)
        {
            Debug.LogError("OrderTakingManager not found.");
            return;
        }

        if (OrderTakingManager.Instance.currentOrder == null)
        {
            Debug.LogWarning("No active order.");
            return;
        }

        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);

        isOpen = !isOpen;

        orderPanel.SetActive(isOpen);

        if (isOpen)
        {
            orderBubbleUI.DisplayOrder(OrderTakingManager.Instance.currentOrder);
        }
    }
}
