using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PackingManager : MonoBehaviour
{
    [System.Serializable]
    public class FlowerEntry
    {
        public ItemsSOScript flowerData;
        public GameObject flowerObject;

        [HideInInspector] public Vector3 startPos;
        [HideInInspector] public Quaternion startRot;
    }

    [Header("Hybrid Flower Slots UI")]
    public Button[] hybridSlots;

    [Header("Flower Gameplay Objects")]
    public FlowerEntry[] flowers;

    //public GameObject hybridLacone;
    //public GameObject hybridForlaven;

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

    [Header("Order Completed Prompts")]
    [SerializeField] public GameObject CorrectOrderPrompt;
    [SerializeField] public GameObject WrongOrderPrompt;
    [SerializeField] public GameObject LastCorrectOrderPrompt;
    [SerializeField] public GameObject LastWrongOrderPrompt;

    [Header("Flower Placements")]
    public FlowerPlacementsSOScript[] flowerPlacements;

    Vector3 accessory1StartPos;
    Vector3 accessory2StartPos;

    List<ItemsSOScript> bouquetFlowers = new List<ItemsSOScript>();

    public bool pluckingInProgress = false;
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

        foreach (var f in flowers)
        {
            f.startPos = f.flowerObject.transform.position;
            f.startRot = f.flowerObject.transform.rotation;

            f.flowerObject.SetActive(false);
        }

        accessory1StartPos = accessory1Object.transform.localPosition;
        accessory2StartPos = accessory2Object.transform.localPosition;

        CorrectOrderPrompt.SetActive(false);
        WrongOrderPrompt.SetActive(false);
        LastCorrectOrderPrompt.SetActive(false);
        LastWrongOrderPrompt.SetActive(false);

        wrapAccessoryTabBackground.SetActive(false);
        SetWrapButtons(false);
        SetAccessoryButtons(false);
        orderCompleteButton.interactable = false;

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
            if (slotIndex >= hybridSlots.Length)
                return;

            Button slotButton = hybridSlots[slotIndex];
            slotButton.gameObject.SetActive(true);
            slotButton.image.sprite = stack.item.itemSprite;

            // ADD TEXT
            Text qtyText = slotButton.GetComponentInChildren<Text>();
            if (qtyText != null)
                qtyText.text = "x" + stack.amount;

            ItemsSOScript flowerData = stack.item;
            slotButton.onClick.AddListener(() => ActivateFlowerFromInventory(flowerData));

            slotIndex++;
        }
    }

    // =========================
    // FLOWER SPAWN
    // =========================
    public void ActivateFlowerFromInventory(ItemsSOScript flowerData)
    {
        if (pluckingInProgress)
        {
            Debug.Log("Finish plucking current flower first.");
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Finish plucking current flower first.");
            return;
        }

        if (wrapSelected || accessorySelected)
        {
            Debug.Log("Remove wrap/accessory first before adding another flower.");
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Remove wrap/accessory first before adding another flower.");
            return;
        }

        if (bouquetFlowers.Count >= maxBouquetFlowers)
        {
            Debug.Log("Bouquet already has 2 flowers.");
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Bouquet already has 2 flowers.");
            return;
        }

        GameObject flowerObj = GetAvailableFlowerObject(flowerData);
        if (flowerObj == null)
        {
            Debug.LogWarning("No available flower GameObject for this flower data.");
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("No more flowers available.");
            return;
        }

        bouquetFlowers.Add(flowerData);
        OrderTakingManager.Instance.currentBouquet.flowers = new List<ItemsSOScript>(bouquetFlowers);

        flowerObj.SetActive(true);
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);

        InventoryManager.Instance.RemoveHybrid(flowerData);
        DisplayHybridInventory();

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
        foreach (var f in flowers)
        {
            if (f.flowerObject == flowerObj)
            {
                flowerObj.transform.position = f.startPos;
                flowerObj.transform.rotation = f.startRot;

                DragFlower drag = flowerObj.GetComponent<DragFlower>();
                if (drag != null)
                    drag.SetHomeTransform(f.startPos, f.startRot);

                SetFlowerOpacity(flowerObj, 1f);
                return;
            }
        }
    }

    Dictionary<ItemsSOScript, int> flowerIndexTracker = new Dictionary<ItemsSOScript, int>();

    GameObject GetAvailableFlowerObject(ItemsSOScript flowerData)
    {
        // get all matching flowers
        List<FlowerEntry> matching = new List<FlowerEntry>();

        foreach (var f in flowers)
        {
            if (f.flowerData == flowerData)
                matching.Add(f);
        }

        if (matching.Count == 0)
            return null;

        // get index
        if (!flowerIndexTracker.ContainsKey(flowerData))
            flowerIndexTracker[flowerData] = 0;

        int startIndex = flowerIndexTracker[flowerData];

        // loop through list
        for (int i = 0; i < matching.Count; i++)
        {
            int index = (startIndex + i) % matching.Count;

            if (!matching[index].flowerObject.activeSelf)
            {
                flowerIndexTracker[flowerData] = (index + 1) % matching.Count;
                return matching[index].flowerObject;
            }
        }

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
        List<GameObject> activeFlowers = new List<GameObject>();

        foreach (var f in flowers)
        {
            if (f.flowerObject.activeSelf)
                activeFlowers.Add(f.flowerObject);
        }

        int activeCount = activeFlowers.Count;

        // 1 flower → original position
        if (activeCount == 1)
        {
            ResetFlowerToOriginalTransform(activeFlowers[0]);
            return;
        }

        // 2 flowers → placement system
        for (int i = 0; i < activeFlowers.Count; i++)
        {
            GameObject flowerObj = activeFlowers[i];
            HybridFlowerTag tag = flowerObj.GetComponent<HybridFlowerTag>();

            if (tag != null)
            {
                ApplyFlowerPlacement(flowerObj, tag.flowerItemData, i);
            }
        }

        UpdateFlowerOpacity();
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

        // If both flowers exist, restore full opacity
        if (bouquetFlowers.Count >= 2)
        {
            foreach (var f in flowers)
            {
                if (f.flowerObject.activeSelf)
                    SetFlowerOpacity(f.flowerObject, 1f);
            }
        }

        Debug.Log("Leaves finished. You may wrap now or add another flower.");
        if (UIManager.Instance != null)
            UIManager.Instance.ShowMessage("You may wrap now or add another flower.");
    }

    public GameObject wrapAccessoryTab;
    public GameObject flowerTab;

    public void ClickToWrapAccessoryTab()
    {
        if (pluckingInProgress == false)
        {
            wrapAccessoryTab.SetActive(true);
            wrapAccessoryTabBackground.SetActive(true);
        }
        
    }

    public void ClickToFlowerTab()
    {
        if (pluckingInProgress == false)
        {
            flowerTab.SetActive(true);
            wrapAccessoryTab.SetActive(false);
            wrapAccessoryTabBackground.SetActive(false);
        }

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

        bool isCorrect = flowerCorrect && wrapCorrect && accessoryCorrect;
        bool isLastCustomer = OrderTakingManager.Instance.currentCustomerIndex == 3;

        if (isCorrect)
        {
            Debug.Log("Order completed successfully!");
            Debug.Log("Adding score for customer");
            ScoreManager.Instance.AddPoints(10);

            if (isLastCustomer)
            {
                LastCorrectOrderPrompt.SetActive(true);
                Debug.Log("Last Correct Order Prompt");
            }
            else
            {
                CorrectOrderPrompt.SetActive(true);
                Debug.Log("Correct Order Prompt");
            }
                
        }
        else
        {
            Debug.Log("Order incorrect!");

            if (isLastCustomer)
            {
                LastWrongOrderPrompt.SetActive(true);
                Debug.Log("Last Wrong Order Prompt");
            }
            else
            {
                WrongOrderPrompt.SetActive(true);
                Debug.Log("Correct Wrong Prompt");
            }
        }

        foreach (var flower in bouquetFlowers)
        {
            InventoryManager.Instance.RemoveHybrid(flower);
        }

        DisplayHybridInventory();
        orderCompleteButton.interactable = false;
    }

    public SceneLoader sceneLoader;
    public void OnContinueAfterOrder()
    {
        CorrectOrderPrompt.SetActive(false);
        WrongOrderPrompt.SetActive(false);

        OrderTakingManager.Instance.FinishOrder();

        if (OrderTakingManager.Instance.IsAfterLastCustomer())
        {
            // Show score screen instead of going back to order scene
            ScoreManager.Instance.ShowFinalScore(); // or load a score scene
        }
        else
        {
            sceneLoader.LoadMainGameScene(); // your normal flow
        }
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
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Accessory removed");
            return;
        }

        if (disposed == wrap.gameObject)
        {
            if (accessorySelected)
            {
                Debug.Log("Remove accessory first!");
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("Remove accessory first!");
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
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Wrap removed");
            return;
        }

        foreach (var f in flowers)
        {
            if (disposed == f.flowerObject)
            {
                if (wrapSelected || accessorySelected)
                {
                    Debug.Log("Remove accessory and wrap first!");
                    if (UIManager.Instance != null)
                        UIManager.Instance.ShowMessage("Remove accessory and wrap first!");
                    return;
                }
                RemoveFlowerFromBouquet(disposed);
                Debug.Log("Flower removed");
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("Flower removed");
                return;
            }
        }
    }

    void RemoveFlowerFromBouquet(GameObject flowerObj)
    {
        HybridFlowerTag tag = flowerObj.GetComponent<HybridFlowerTag>();
        if (tag == null) return;

        bouquetFlowers.Remove(tag.flowerItemData);
        OrderTakingManager.Instance.currentBouquet.flowers = new List<ItemsSOScript>(bouquetFlowers);

        flowerObj.SetActive(false);

        ResetLeaves(flowerObj);
        RelayoutActiveFlowers();

        pluckingInProgress = false;
        CheckIfOrderReady();
    }

    public void DisposeWholeBouquet()
    {
        Debug.Log("Whole bouquet disposed");
        if (UIManager.Instance != null)
            UIManager.Instance.ShowMessage("Bouquet disposed!");
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

        foreach (var f in flowers)
        {
            SetFlowerOpacity(f.flowerObject, 1f);
            f.flowerObject.SetActive(false);
        }

        foreach (var f in flowers)
        {
            ResetLeaves(f.flowerObject); // reset EACH flower
        }

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
        if (saved == null)
        {
            Debug.Log("no bouquet saved");
        }

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

    void ResetLeaves(GameObject flowerObj)
    {
        LeafTracker tracker = FindObjectOfType<LeafTracker>();
        if (tracker != null)
        {
            tracker.ResetLeaves();
        }
    }

    void DisableFlowerDragging()
    {
        foreach (var f in flowers)
        {
            DragFlower drag = f.flowerObject.GetComponent<DragFlower>();
            if (drag != null)
                drag.enabled = false;
        }
    }

    void EnableFlowerDragging()
    {
        foreach (var f in flowers)
        {
            DragFlower drag = f.flowerObject.GetComponent<DragFlower>();
            if (drag != null)
                drag.enabled = true;
        }
    }

    void SaveBouquetState()
    {
        OrderTakingManager.Instance.currentBouquet.flowers = new List<ItemsSOScript>(bouquetFlowers);
        OrderTakingManager.Instance.currentBouquet.wrap = selectedWrap;
        OrderTakingManager.Instance.currentBouquet.accessory = selectedAccessory;
    }

    void SetFlowerOpacity(GameObject flowerObj, float alpha)
    {
        if (flowerObj == null) return;

        SpriteRenderer[] renderers = flowerObj.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer sr in renderers)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
    }

    void UpdateFlowerOpacity()
    {
        List<GameObject> activeFlowers = new List<GameObject>();

        foreach (var flowerData in bouquetFlowers)
        {
            foreach (var f in flowers)
            {
                if (f.flowerData == flowerData && f.flowerObject.activeSelf)
                {
                    activeFlowers.Add(f.flowerObject);
                    break;
                }
            }
        }

        for (int i = 0; i < activeFlowers.Count; i++)
        {
            float alpha = (i == 0 && activeFlowers.Count >= 2) ? 0.5f : 1f;
            SetFlowerOpacity(activeFlowers[i], alpha);
        }
        //if (activeFlowers.Count == 1)
        //{
        //    SetFlowerOpacity(activeFlowers[0], 1f);
        //}
        //else if (activeFlowers.Count >= 2)
        //{
        //    SetFlowerOpacity(activeFlowers[0], 0.5f);
        //    SetFlowerOpacity(activeFlowers[1], 1f);
        //}
    }
}