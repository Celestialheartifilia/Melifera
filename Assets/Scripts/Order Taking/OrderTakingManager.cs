using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderTakingManager : MonoBehaviour
{
    public List<ItemsSOScript> normalFlowerItems;
    public List<ItemsSOScript> hybridFlowerItems;
    public List<ItemsSOScript> wrapItems;
    public List<ItemsSOScript> accessoryItems;

    public OrderList currentOrder;

    public int currentCustomerIndex = 0;
    public int maxCustomers = 3;

    //public NextOrderPopup nextOrderPopup;

    //public OrderBubbleUI orderBubbleUI;

    public static OrderTakingManager Instance;

    ////take order button
    //public GameObject takeOrderButton;
    //// Pop up message 
    //public GameObject collectedPopup;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public enum OrderType
    {
        Small,
        Medium,
        Big
    }

    // Call this when a new customer appears
    public void CreateNewOrder(OrderType orderType)
    {
        currentOrder = new OrderList();

        switch (orderType)
        {
            case OrderType.Small:
                CreateSmallOrder();
                break;

            case OrderType.Medium:
                CreateMediumOrder();
                break;

            case OrderType.Big:
                CreateBigOrder();
                break;
        }

        Debug.Log("New order created.");

        //orderBubbleUI.DisplayOrder(currentOrder);
        // HIDE the bubble initially (so it's not showing yet)
        //orderBubbleUI.gameObject.SetActive(false);
        //SHOW the button 
        //if (takeOrderButton != null) takeOrderButton.SetActive(true);
    }

    public bool HasActiveOrder()
    {
        return currentOrder != null;
    }

    public bool HasMoreCustomers()
    {
        return currentCustomerIndex < maxCustomers;
    }

    public void FinishOrder()
    {
        currentOrder = null;
        //nextOrderPopup.ShowPopupAfterDelay();
    }

    //public void OnTakeOrderButtonClicked()
    //{
    //    if (currentOrder != null)
    //    {
    //        // SHOW the bubble now that we clicked
    //        orderBubbleUI.gameObject.SetActive(true);
    //        orderBubbleUI.DisplayOrder(currentOrder);

    //        // HIDE the button
    //        if (takeOrderButton != null) takeOrderButton.SetActive(false);
    //        Debug.Log("Order collected!");
    //    }
    //}

    //public void OnTakeOrderButtonClicked()
    //{
    //    if (currentOrder != null)
    //    {
    //        OrderUIManager.Instance.ShowOrder(currentOrder);
    //        Debug.Log("Order collected!");
    //    }
    //}

    //public void OnCloseOrderBubble()
    //{
    //    OrderUIManager.Instance.CloseOrderBubble();
    //}

    //public void OnCloseOrderBubble()
    //{  
    //    //The Bubble disappears first
    //    if (orderBubbleUI != null)
    //    {
    //        orderBubbleUI.gameObject.SetActive(false);
    //    }
    //    // The Popup message starts appearing immediately after
    //    if (collectedPopup != null)
    //    {

    //        collectedPopup.SetActive(true);
    //        //StartCoroutine(FlashMessage());
    //    }

    //    Debug.Log("Bubble gone, Popup shown!");
    //}



    // Call this to add an item to the order
    void CreateSmallOrder()
    {
        // 1 random hybrid flower 
        AddRandomHybridFlower();

        // 1 random wrap
        AddRandomWrap();

        // 1 random accessory
        AddRandomAccessory();
        Debug.Log("all items r ordered");
    }

    void CreateMediumOrder()
    {
        // 1 random hybrid flower
        AddRandomHybridFlower();

        // 1 random normal flower
        AddRandomNormalFlower();

        // 1 random wrap
        AddRandomWrap();

        // 1 random accessory
        AddRandomAccessory();
    }

    void CreateBigOrder()
    {
        // 2 random different hybrid flower
        ItemsSOScript randomHybridFlower1 = hybridFlowerItems[Random.Range(0, hybridFlowerItems.Count)];
        currentOrder.orderedItems.Add(randomHybridFlower1);

        ItemsSOScript randomHybridFlower2;
        do
        {
            randomHybridFlower2 = hybridFlowerItems[Random.Range(0, hybridFlowerItems.Count)];
        }
        while (randomHybridFlower2 == randomHybridFlower1);

        currentOrder.orderedItems.Add(randomHybridFlower2);

        // 1 random wrap
        AddRandomWrap();

        // 1 random accessory
        AddRandomAccessory();
    }

    #region Add random items to order methods
    void AddRandomHybridFlower()
    {
        ItemsSOScript randomHybridFlower = hybridFlowerItems[Random.Range(0, hybridFlowerItems.Count)];
        currentOrder.orderedItems.Add(randomHybridFlower);
        Debug.Log(randomHybridFlower);
    }

    void AddRandomNormalFlower()
    {
        ItemsSOScript randomNormalFlower = normalFlowerItems[Random.Range(0, normalFlowerItems.Count)];
        currentOrder.orderedItems.Add(randomNormalFlower);
        Debug.Log(randomNormalFlower);
    }

    void AddRandomWrap()
    {
        ItemsSOScript randomWrap = wrapItems[Random.Range(0, wrapItems.Count)];
        currentOrder.orderedItems.Add(randomWrap);
        Debug.Log(randomWrap);
    }

    void AddRandomAccessory()
    {
        ItemsSOScript randomAccessory = accessoryItems[Random.Range(0, accessoryItems.Count)];
        currentOrder.orderedItems.Add(randomAccessory);
        Debug.Log(randomAccessory);
    }
    #endregion

    // Call this when the order is done
    public void ClearOrder()
    {
        currentOrder = null;
    }

    //private System.Collections.IEnumerator FlashMessage()
    //{
    //    collectedPopup.SetActive(true);
    //    yield return new WaitForSeconds(2f);
    //    collectedPopup.SetActive(false);

    //    // --- NEW: Load the scene here ---
    //    Debug.Log("Transitioning to Main Game Scene...");
    //    SceneManager.LoadScene("MainGameScene"); 
    //}
    
}
