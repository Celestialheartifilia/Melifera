using UnityEngine;

public class OrderUIManager : MonoBehaviour
{
    public OrderBubbleUI orderBubbleUI;
    public GameObject takeOrderButton;
    public GameObject collectedPopup;

    void Start()
    {
        if (orderBubbleUI != null)
            orderBubbleUI.gameObject.SetActive(false);

        if (takeOrderButton != null)
            takeOrderButton.SetActive(OrderTakingManager.Instance != null &&
                                      OrderTakingManager.Instance.currentOrder != null);

        if (collectedPopup != null)
            collectedPopup.SetActive(false);
    }

    public void OnTakeOrderButtonClicked()
    {
        
        if (OrderTakingManager.Instance == null || OrderTakingManager.Instance.currentOrder == null)
            return;

        Debug.Log("Take order clicked");


        if (orderBubbleUI != null)
        {
            orderBubbleUI.gameObject.SetActive(true);
            orderBubbleUI.DisplayOrder(OrderTakingManager.Instance.currentOrder);
        }

        if (takeOrderButton != null)
            takeOrderButton.SetActive(false);


        Debug.Log("Order collected!");
    }

    public void OnCloseOrderBubble()
    {
        if (orderBubbleUI != null)
            orderBubbleUI.gameObject.SetActive(false);

        if (collectedPopup != null)
            collectedPopup.SetActive(true);

        Debug.Log("Bubble gone, popup shown!");
    }

    public void RefreshUIForNewOrder()
    {
        if (orderBubbleUI != null)
            orderBubbleUI.gameObject.SetActive(false);

        if (collectedPopup != null)
            collectedPopup.SetActive(false);

        if (takeOrderButton != null)
            takeOrderButton.SetActive(OrderTakingManager.Instance != null &&
                                      OrderTakingManager.Instance.currentOrder != null);
    }
}
