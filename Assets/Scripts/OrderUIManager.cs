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
        if (OrderTakingManager.Instance == null)
            return;

        var manager = OrderTakingManager.Instance;

        if (!manager.hasTakenOrder)
        {
            manager.CreateNewOrder(manager.pendingOrderType);
            manager.hasTakenOrder = true;

            // order taking SFX
            if (SoundEffectPlayer.Instance != null)
                SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.orderTakingSFX);
        }

        if (orderBubbleUI != null)
        {
            orderBubbleUI.gameObject.SetActive(true);
            orderBubbleUI.DisplayOrder(manager.currentOrder);
        }

        if (takeOrderButton != null)
            takeOrderButton.SetActive(false);
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
        var manager = OrderTakingManager.Instance;

        if (orderBubbleUI != null)
            orderBubbleUI.gameObject.SetActive(false);

        if (collectedPopup != null)
            collectedPopup.SetActive(false);

        if (manager == null) return;

        // KEY LOGIC
        if (manager.hasTakenOrder && manager.currentOrder != null)
        {
            // show existing order
            if (orderBubbleUI != null)
            {
                orderBubbleUI.gameObject.SetActive(true);
                orderBubbleUI.DisplayOrder(manager.currentOrder);
            }

            if (takeOrderButton != null)
                takeOrderButton.SetActive(false);
        }
        else
        {
            // show take order button
            if (takeOrderButton != null)
                takeOrderButton.SetActive(true);
        }
    }
}