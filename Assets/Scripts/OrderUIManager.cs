using UnityEngine;

public class OrderUIManager : MonoBehaviour
{
    public OrderBubbleUI orderBubbleUI;
    public GameObject takeOrderButton;
    public GameObject collectedPopup;
    public SceneLoader sceneLoader;

    Camera cam;

    void Start()
    {
        cam = Camera.main;

        //if (orderBubbleUI != null)
        //    orderBubbleUI.gameObject.SetActive(false);

        //if (takeOrderButton != null)
        //    takeOrderButton.SetActive(OrderTakingManager.Instance != null &&
        //                              OrderTakingManager.Instance.currentOrder != null);

        //if (collectedPopup != null)
        //    collectedPopup.SetActive(false);

        RefreshUIForNewOrder();
    }

    void Update()
    {
        if (orderBubbleUI == null) return;
        if (!orderBubbleUI.gameObject.activeSelf) return;

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

        sceneLoader.LoadMainGameScene();

        Debug.Log("Bubble gone!");
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