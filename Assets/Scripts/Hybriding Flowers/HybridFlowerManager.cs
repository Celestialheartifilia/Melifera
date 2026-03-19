using System.Collections.Generic;
using UnityEngine;

public class HybridFlowerManager : MonoBehaviour
{
    //Fetches current order data at start
    private OrderTakingManager orderTakingManager;

    [Header("Order Data")]
    public List<ItemsSOScript> remainingRequiredHybrids = new();

    [Header("Hybrid Flowers in Scene")]
    public GameObject hybridFlower1;
    public GameObject hybridFlower2;


    public GameObject speechIndicator;
    public GameObject speechIndicator2;

    void Awake()
    {
        if (speechIndicator != null)
            speechIndicator.SetActive(false);
        if (speechIndicator2 != null)
            speechIndicator2.SetActive(false);
    }
    void Start()
    {
        orderTakingManager = OrderTakingManager.Instance;
        remainingRequiredHybrids.Clear();

        if (orderTakingManager == null || orderTakingManager.currentOrder == null)
        {
            Debug.LogWarning("No order found.");
            return;
        }

        foreach (var item in orderTakingManager.currentOrder.orderedItems)
        {
            if (orderTakingManager.hybridFlowerItems.Contains(item))
            {
                remainingRequiredHybrids.Add(item);
            }
        }

        // FORCE hide both at start
        hybridFlower1.SetActive(false);
        hybridFlower2.SetActive(false);
    }

    //CALLED BY Pot.cs
    public void OnHybridReadyToCut(ItemsSOScript hybrid)
    {
        if (hybrid == null)
        {
            Debug.LogError("Hybrid passed is NULL");
            return;
        }

        Debug.Log($"Hybrid ready: {hybrid.itemName} | ID: {hybrid.itemID}");

        ShowHybrid(hybrid);
    }

    void ShowHybrid(ItemsSOScript hybridData)
    {
        Debug.Log("[MANAGER] ShowHybrid called for " + hybridData.itemName);

        hybridFlower1.SetActive(false);
        hybridFlower2.SetActive(false);

        var cut1 = hybridFlower1.GetComponentInChildren<FlowerCutSwap>();
        var cut2 = hybridFlower2.GetComponentInChildren<FlowerCutSwap>();

        if (cut1 == null || cut2 == null)
        {
            Debug.LogError("FlowerCutSwap missing on hybrid flowers");
            return;
        }

        Debug.Log($"[MANAGER] Comparing {hybridData.itemID} against:");
        Debug.Log($"Hybrid1 = {cut1.flowerItemData.itemID}");
        Debug.Log($"Hybrid2 = {cut2.flowerItemData.itemID}");

        if (cut1.flowerItemData.itemID == hybridData.itemID)
        {
            hybridFlower1.SetActive(true);
            Debug.Log("[MANAGER] Activated HybridFlower1");
        }
        else if (cut2.flowerItemData.itemID == hybridData.itemID)
        {
            hybridFlower2.SetActive(true);
            Debug.Log("[MANAGER] Activated HybridFlower2");
        }
        else
        {
            Debug.LogError("[MANAGER] No hybrid matched!");
        }
    }
}
