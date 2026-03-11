using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PackingManager : MonoBehaviour
{
    [Header("Hybrid Flower Slots UI")]
    public Button[] hybridSlots;

    [Header("Flower Gameplay Objects")]

    public GameObject hybridLacone;
    public GameObject hybridForlaven;

    [Header("Wrap Visual")]
    public SpriteRenderer wrapBackRenderer;
    public SpriteRenderer wrapFrontRenderer;
    public ItemsSOScript wrap1;
    public ItemsSOScript wrap2;
    public Sprite wrap1BackSprite;
    public Sprite wrap1FrontSprite;
    public Sprite wrap2BackSprite;
    public Sprite wrap2FrontSprite;
    public GameObject wrap;

    [Header("Accessory Visual")]
    public ItemsSOScript accessory1;
    public ItemsSOScript accessory2;
    public GameObject accessory1Object;
    public GameObject accessory2Object;

    [Header("Wrap + Accessory Buttons")]
    public GameObject wrapAccessoryTabBackground;
    public Button wrap1Button;
    public Button wrap2Button;
    public Button accessory1Button;
    public Button accessory2Button;

    [Header("Order")]
    public Button orderCompleteButton;

    [Header("UI Order")]
    [SerializeField] GameObject CorrectOrderPrompt;
    [SerializeField] GameObject WrongOrderPrompt;

    [Header("Flower Placements")]
    public FlowerPlacementsSOScript[] flowerPlacements;

    Vector3 hybridLaconeStartPos;
    Vector3 hybridForlavenStartPos;

    Quaternion hybridLaconeStartRot;
    Quaternion hybridForlavenStartRot;

    Vector3 accessory1StartPos;
    Vector3 accessory2StartPos;

    List<ItemsSOScript> bouquetFlowers = new List<ItemsSOScript>();

    bool pluckingInProgress = false;
    bool wrapSelected = false;
    bool accessorySelected = false;

    ItemsSOScript selectedWrap;
    ItemsSOScript selectedAccessory;

    const int maxBouquetFlowers = 2;

    void Start()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager missing.");
            return;
        }

        hybridLaconeStartPos = hybridLacone.transform.position;
        hybridForlavenStartPos = hybridForlaven.transform.position;

        hybridLaconeStartRot = hybridLacone.transform.rotation;
        hybridForlavenStartRot = hybridForlaven.transform.rotation;

        accessory1StartPos = accessory1Object.transform.localPosition;
        accessory2StartPos = accessory2Object.transform.localPosition;

        CorrectOrderPrompt.SetActive(false);
        WrongOrderPrompt.SetActive(false);

        wrapAccessoryTabBackground.SetActive(false);
        SetWrapButtons(false);
        SetAccessoryButtons(false);
        orderCompleteButton.interactable = false;

        hybridLacone.SetActive(false);
        hybridForlaven.SetActive(false);

        DisplayHybridInventory();
        RestoreSavedBouquet();
    }

    // =========================
    // INVENTORY UI
    // =========================
    void DisplayHybridInventory()
    {
        for (int i = 0; i < hybridSlots.Length; i++)
        {
            hybridSlots[i].gameObject.SetActive(false);
            hybridSlots[i].onClick.RemoveAllListeners();
        }

        int slotIndex = 0;

        foreach (var stack in InventoryManager.Instance.hybrids)
        {
            for (int i = 0; i < stack.amount; i++)
            {
                if (slotIndex >= hybridSlots.Length)
                    return;

                Button slotButton = hybridSlots[slotIndex];
                slotButton.gameObject.SetActive(true);
                slotButton.image.sprite = stack.item.itemSprite;

                ItemsSOScript flowerData = stack.item;
                slotButton.onClick.AddListener(() => ActivateFlowerFromInventory(flowerData));

                slotIndex++;
            }
        }
    }

    // =========================
    // FLOWER SPAWN
    // =========================
    void ActivateFlowerFromInventory(ItemsSOScript flowerData)
    {
        if (pluckingInProgress)
        {
            Debug.Log("Finish plucking current flower first.");
            return;
        }

        if (wrapSelected || accessorySelected)
        {
            Debug.Log("Remove wrap/accessory first before adding another flower.");
            return;
        }

        if (bouquetFlowers.Count >= maxBouquetFlowers)
        {
            Debug.Log("Bouquet already has 2 flowers.");
            return;
        }

        GameObject flowerObj = GetAvailableFlowerObject(flowerData);
        if (flowerObj == null)
        {
            Debug.LogWarning("No available flower GameObject for this flower data.");
            return;
        }

        bouquetFlowers.Add(flowerData);
        OrderTakingManager.Instance.currentBouquet.flowers = new List<ItemsSOScript>(bouquetFlowers);

        flowerObj.SetActive(true);

        // If this is the first flower, keep its original transform
        if (bouquetFlowers.Count == 1)
        {
            ResetFlowerToOriginalTransform(flowerObj);
        }
        // If now there are 2 flowers, arrange both using placement SO
        else if (bouquetFlowers.Count == 2)
        {
            RelayoutActiveFlowers();
        }

        pluckingInProgress = true;

        Debug.Log("Flower added. Pluck leaves first.");

        SaveBouquetState();
    }

    void ResetFlowerToOriginalTransform(GameObject flowerObj)
    {
        if (flowerObj == hybridLacone)
        {
            hybridLacone.transform.position = hybridLaconeStartPos;
            hybridLacone.transform.rotation = hybridLaconeStartRot;

            DragFlower drag = hybridLacone.GetComponent<DragFlower>();
            if (drag != null)
                drag.SetHomeTransform(hybridLaconeStartPos, hybridLaconeStartRot);
        }
        else if (flowerObj == hybridForlaven)
        {
            hybridForlaven.transform.position = hybridForlavenStartPos;
            hybridForlaven.transform.rotation = hybridForlavenStartRot;

            DragFlower drag = hybridForlaven.GetComponent<DragFlower>();
            if (drag != null)
                drag.SetHomeTransform(hybridForlavenStartPos, hybridForlavenStartRot);
        }
    }

    GameObject GetAvailableFlowerObject(ItemsSOScript flowerData)
    {
        HybridFlowerTag tag1 = hybridLacone.GetComponent<HybridFlowerTag>();
        HybridFlowerTag tag2 = hybridForlaven.GetComponent<HybridFlowerTag>();

        if (tag1 != null && tag1.flowerItemData == flowerData && !hybridLacone.activeSelf)
            return hybridLacone;

        if (tag2 != null && tag2.flowerItemData == flowerData && !hybridForlaven.activeSelf)
            return hybridForlaven;

        return null;
    }

    // =========================
    // FLOWER PLACEMENT (SO)
    // =========================
    FlowerPlacementsSOScript GetPlacementData(ItemsSOScript flower)
    {
        foreach (var placement in flowerPlacements)
        {
            if (placement.flowerItem == flower)
                return placement;
        }

        return null;
    }

    void ApplyFlowerPlacement(GameObject flowerObj, ItemsSOScript flowerData, int bouquetIndex)
    {
        FlowerPlacementsSOScript placement = GetPlacementData(flowerData);

        if (placement == null)
        {
            Debug.LogWarning("No placement data for " + flowerData.name);
            return;
        }

        Vector3 targetPos;
        Quaternion targetRot;

        if (bouquetIndex == 0)
        {
            targetPos = placement.firstPosition;
            targetRot = Quaternion.Euler(placement.firstRotation);
        }
        else
        {
            targetPos = placement.secondPosition;
            targetRot = Quaternion.Euler(placement.secondRotation);
        }

        flowerObj.transform.position = targetPos;
        flowerObj.transform.rotation = targetRot;

        DragFlower drag = flowerObj.GetComponent<DragFlower>();
        if (drag != null)
            drag.SetHomeTransform(targetPos, targetRot);
    }

    void RelayoutActiveFlowers()
    {
        int activeCount = 0;

        if (hybridLacone.activeSelf) activeCount++;
        if (hybridForlaven.activeSelf) activeCount++;

        // Only 1 flower -> keep original position
        if (activeCount == 1)
        {
            if (hybridLacone.activeSelf) ResetFlowerToOriginalTransform(hybridLacone);
            if (hybridForlaven.activeSelf) ResetFlowerToOriginalTransform(hybridForlaven);
            return;
        }

        // 2 flowers -> use placement SO
        int activeIndex = 0;

        if (hybridLacone.activeSelf)
        {
            ItemsSOScript data = hybridLacone.GetComponent<HybridFlowerTag>().flowerItemData;
            ApplyFlowerPlacement(hybridLacone, data, activeIndex);
            activeIndex++;
        }

        if (hybridForlaven.activeSelf)
        {
            ItemsSOScript data = hybridForlaven.GetComponent<HybridFlowerTag>().flowerItemData;
            ApplyFlowerPlacement(hybridForlaven, data, activeIndex);
            activeIndex++;
        }
    }

    // =========================
    // LEAF COMPLETE
    // =========================
    public void OnLeavesPlucked()
    {
        pluckingInProgress = false;

        wrapAccessoryTabBackground.SetActive(true);
        SetWrapButtons(true);
        SetAccessoryButtons(false);

        Debug.Log("Leaves finished. You may wrap now or add another flower.");
    }

    void SetWrapButtons(bool state)
    {
        wrap1Button.gameObject.SetActive(state);
        wrap2Button.gameObject.SetActive(state);
    }

    void SetAccessoryButtons(bool state)
    {
        accessory1Button.gameObject.SetActive(state);
        accessory2Button.gameObject.SetActive(state);
    }

    // =========================
    // WRAP
    // =========================
    public void SelectWrap1()
    {
        selectedWrap = wrap1;
        wrapSelected = true;
        OrderTakingManager.Instance.currentBouquet.wrap = selectedWrap;

        DisableFlowerDragging();

        wrapBackRenderer.sprite = wrap1BackSprite;
        wrapFrontRenderer.sprite = wrap1FrontSprite;

        SetAccessoryButtons(true);
        CheckIfOrderReady();
    }

    public void SelectWrap2()
    {
        selectedWrap = wrap2;
        wrapSelected = true;
        OrderTakingManager.Instance.currentBouquet.wrap = selectedWrap;

        DisableFlowerDragging();

        wrapBackRenderer.sprite = wrap2BackSprite;
        wrapFrontRenderer.sprite = wrap2FrontSprite;

        SetAccessoryButtons(true);
        CheckIfOrderReady();
    }

    // =========================
    // ACCESSORY
    // =========================
    public void SelectAccessory1()
    {
        if (!wrapSelected) return;

        selectedAccessory = accessory1;
        accessorySelected = true;
        OrderTakingManager.Instance.currentBouquet.accessory = selectedAccessory;

        wrap.GetComponent<DragReturn>().enabled = false;

        accessory1Object.SetActive(true);
        accessory2Object.SetActive(false);

        CheckIfOrderReady();
    }

    public void SelectAccessory2()
    {
        if (!wrapSelected) return;

        selectedAccessory = accessory2;
        accessorySelected = true;
        OrderTakingManager.Instance.currentBouquet.accessory = selectedAccessory;

        wrap.GetComponent<DragReturn>().enabled = false;

        accessory1Object.SetActive(false);
        accessory2Object.SetActive(true);

        CheckIfOrderReady();
    }

    void CheckIfOrderReady()
    {
        if (selectedWrap != null && selectedAccessory != null && bouquetFlowers.Count > 0 && !pluckingInProgress)
            orderCompleteButton.interactable = true;
        else
            orderCompleteButton.interactable = false;
    }

    public void OnOrderComplete()
    {
        ValidateOrder();
    }

    // =========================
    // VALIDATION
    // =========================
    void ValidateOrder()
    {
        var order = OrderTakingManager.Instance.currentOrder;

        bool flowerCorrect = ValidateFlowersExactly(order);
        bool wrapCorrect = order.orderedItems.Contains(selectedWrap);
        bool accessoryCorrect = order.orderedItems.Contains(selectedAccessory);

        if (flowerCorrect && wrapCorrect && accessoryCorrect)
        {
            Debug.Log("Order completed successfully!");
            CorrectOrderPrompt.SetActive(true);
        }
        else
        {
            Debug.Log("Order incorrect!");
            WrongOrderPrompt.SetActive(true);
        }

        foreach (var flower in bouquetFlowers)
        {
            InventoryManager.Instance.RemoveHybrid(flower);
        }

        DisplayHybridInventory();
        OrderTakingManager.Instance.FinishOrder();
    }

    bool ValidateFlowersExactly(OrderList order)
    {
        List<ItemsSOScript> requiredFlowers = new List<ItemsSOScript>();

        foreach (var item in order.orderedItems)
        {
            if (OrderTakingManager.Instance.hybridFlowerItems.Contains(item) ||
                OrderTakingManager.Instance.normalFlowerItems.Contains(item))
            {
                requiredFlowers.Add(item);
            }
        }

        if (bouquetFlowers.Count != requiredFlowers.Count)
            return false;

        List<ItemsSOScript> tempBouquet = new List<ItemsSOScript>(bouquetFlowers);

        foreach (var required in requiredFlowers)
        {
            if (!tempBouquet.Contains(required))
                return false;

            tempBouquet.Remove(required);
        }

        return true;
    }

    // =========================
    // DISPOSAL
    // =========================
    public void HandleDisposal(GameObject disposed)
    {
        if (disposed == accessory1Object || disposed == accessory2Object)
        {
            accessorySelected = false;
            selectedAccessory = null;
            OrderTakingManager.Instance.currentBouquet.accessory = null;

            wrap.GetComponent<DragReturn>().enabled = true;

            accessory1Object.SetActive(false);
            accessory1Object.transform.localPosition = accessory1StartPos;

            accessory2Object.SetActive(false);
            accessory2Object.transform.localPosition = accessory2StartPos;

            orderCompleteButton.interactable = false;
            Debug.Log("Accessory removed");
            return;
        }

        if (disposed == wrap.gameObject)
        {
            if (accessorySelected)
            {
                Debug.Log("Remove accessory first!");
                return;
            }

            wrapSelected = false;
            selectedWrap = null;
            OrderTakingManager.Instance.currentBouquet.wrap = null;

            wrap.GetComponent<DragReturn>().enabled = true;
            EnableFlowerDragging();

            wrapBackRenderer.sprite = null;
            wrapFrontRenderer.sprite = null;

            SetAccessoryButtons(false);
            orderCompleteButton.interactable = false;

            Debug.Log("Wrap removed");
            return;
        }

        if (disposed == hybridLacone || disposed == hybridForlaven)
        {
            if (wrapSelected || accessorySelected)
            {
                Debug.Log("Remove accessory and wrap first!");
                return;
            }

            RemoveFlowerFromBouquet(disposed);
            Debug.Log("Flower removed");
        }
    }

    void RemoveFlowerFromBouquet(GameObject flowerObj)
    {
        HybridFlowerTag tag = flowerObj.GetComponent<HybridFlowerTag>();
        if (tag == null) return;

        bouquetFlowers.Remove(tag.flowerItemData);
        OrderTakingManager.Instance.currentBouquet.flowers = new List<ItemsSOScript>(bouquetFlowers);

        flowerObj.SetActive(false);

        ResetLeaves();
        RelayoutActiveFlowers();

        pluckingInProgress = false;
        CheckIfOrderReady();
    }

    public void DisposeWholeBouquet()
    {
        Debug.Log("Whole bouquet disposed");
        ResetPackingScene();
    }

    // =========================
    // RESET / RESTORE
    // =========================
    void ResetPackingScene()
    {
        bouquetFlowers.Clear();
        OrderTakingManager.Instance.ResetBouquet();

        wrapSelected = false;
        accessorySelected = false;
        pluckingInProgress = false;

        selectedWrap = null;
        selectedAccessory = null;

        hybridLacone.SetActive(false);
        hybridForlaven.SetActive(false);

        ResetLeaves();

        wrapBackRenderer.sprite = null;
        wrapFrontRenderer.sprite = null;

        SetWrapButtons(false);
        SetAccessoryButtons(false);
        wrapAccessoryTabBackground.SetActive(false);

        accessory1Object.SetActive(false);
        accessory1Object.transform.localPosition = accessory1StartPos;

        accessory2Object.SetActive(false);
        accessory2Object.transform.localPosition = accessory2StartPos;

        orderCompleteButton.interactable = false;

        EnableFlowerDragging();
    }

    void RestoreSavedBouquet()
    {
        var saved = OrderTakingManager.Instance.currentBouquet;
        if (saved == null) return;

        bouquetFlowers = new List<ItemsSOScript>(saved.flowers);
        selectedWrap = saved.wrap;
        selectedAccessory = saved.accessory;

        wrapSelected = selectedWrap != null;
        accessorySelected = selectedAccessory != null;

        if (bouquetFlowers.Count > 0)
        {
            for (int i = 0; i < bouquetFlowers.Count; i++)
            {
                GameObject flowerObj = GetAvailableFlowerObject(bouquetFlowers[i]);
                if (flowerObj == null) continue;

                flowerObj.SetActive(true);
            }

            RelayoutActiveFlowers();
        }

        if (selectedWrap == wrap1)
        {
            wrapBackRenderer.sprite = wrap1BackSprite;
            wrapFrontRenderer.sprite = wrap1FrontSprite;
        }
        else if (selectedWrap == wrap2)
        {
            wrapBackRenderer.sprite = wrap2BackSprite;
            wrapFrontRenderer.sprite = wrap2FrontSprite;
        }

        if (selectedAccessory == accessory1)
        {
            accessory1Object.SetActive(true);
            accessory2Object.SetActive(false);
        }
        else if (selectedAccessory == accessory2)
        {
            accessory1Object.SetActive(false);
            accessory2Object.SetActive(true);
        }

        if (bouquetFlowers.Count > 0)
            wrapAccessoryTabBackground.SetActive(true);

        SetWrapButtons(bouquetFlowers.Count > 0);
        SetAccessoryButtons(wrapSelected);
        CheckIfOrderReady();
    }

    void ResetLeaves()
    {
        LeafTracker tracker = FindObjectOfType<LeafTracker>();
        if (tracker != null)
            tracker.ResetLeaves();
    }

    void DisableFlowerDragging()
    {
        DragFlower d1 = hybridLacone.GetComponent<DragFlower>();
        DragFlower d2 = hybridForlaven.GetComponent<DragFlower>();

        if (d1 != null) d1.enabled = false;
        if (d2 != null) d2.enabled = false;
    }

    void EnableFlowerDragging()
    {
        DragFlower d1 = hybridLacone.GetComponent<DragFlower>();
        DragFlower d2 = hybridForlaven.GetComponent<DragFlower>();

        if (d1 != null) d1.enabled = true;
        if (d2 != null) d2.enabled = true;
    }

    void SaveBouquetState()
    {
        OrderTakingManager.Instance.currentBouquet.flowers = new List<ItemsSOScript>(bouquetFlowers);
        OrderTakingManager.Instance.currentBouquet.wrap = selectedWrap;
        OrderTakingManager.Instance.currentBouquet.accessory = selectedAccessory;
    }
}