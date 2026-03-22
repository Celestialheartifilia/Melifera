using UnityEngine;

public class Pot : MonoBehaviour
{
    [Header("Tool Colliders")]
    public Collider2D fertiliserCollider;
    public Collider2D scissorsCollider;
    public GameObject BeeGone;

    [Header("UI (Optional)")]
    public GameObject toolsReadyPopup;

    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Sprite emptyPotSprite;
    public Sprite pollenPotSprite;
    public Sprite potWithStudSprite;

    [Header("Hybrid Flower Visuals")]
    public GameObject hybridFlower1;
    public GameObject hybridFlower2;

    public ItemsSOScript hybrid1Data;
    public ItemsSOScript hybrid2Data;

    //public bool isEmpty = true;
    public enum FlowerGrowthState
    {
        Empty,
        Planted,
        Fertilised,
        Grown
    }

    //store the current state -> in this case, there is no flower = empty
    [HideInInspector] public FlowerGrowthState growthState = FlowerGrowthState.Empty;

    //reference to the hybridflowermanager -> for communication between pot n manager
    public HybridFlowerManager hybridFlowerManager;
    public PollinationManager pollinationManager;
    public FertilizerManager fertilizerManager;

    //to store the hybrid that was planted
    public ItemsSOScript plantedHybrid;

    //initial empty state
    void Start()
    {
        ResetPot();
    }

    public void ResetPot()
    {
        plantedHybrid = null;
        growthState = FlowerGrowthState.Empty;

        if (fertilizerManager != null)
            fertilizerManager.ResetFertiliserState();

        //disables the fertiliser
        if (fertiliserCollider != null)
        {
            fertiliserCollider.enabled = false;
        }

        //disables the scissors
        if (scissorsCollider != null)
        {
            scissorsCollider.enabled = false;
        }

        //hides UI for tools ready pop up
        if (toolsReadyPopup != null)
        {
            toolsReadyPopup.SetActive(false);
        }
     

        if (spriteRenderer != null && emptyPotSprite != null)
        {
            spriteRenderer.sprite = emptyPotSprite;
        }

        Debug.Log("[POT] Reset.");

        pollinationManager.ResetPollination();
    }

    //method is used in the pollination manager script
    public bool Plant(ItemsSOScript hybrid)
    {
        //ensures that theres a hybrid to plant
        if (hybrid == null)
        {
            Debug.LogWarning("[POT] Tried to plant NULL hybrid.");
            return false;
        }
        //ensures the pot is empty
        if (growthState != FlowerGrowthState.Empty)
        {
            Debug.LogWarning("[POT] Tried to plant but pot is not empty.");
            return false;
        }

        //stores the hybrid data
        plantedHybrid = hybrid;

        //changes the state to planted
        growthState = FlowerGrowthState.Planted;

        if (spriteRenderer != null && pollenPotSprite != null)
        {
            spriteRenderer.sprite = pollenPotSprite;
        }
            
        Debug.Log($"[PLANT] Planted hybrid: {hybrid.itemName} | ID: {hybrid.itemID}");

        // Enable fertiliser colliders
        if (fertiliserCollider != null)
        {
            fertiliserCollider.enabled = true;
        }

        //if planting is successful
        return true;

    }
    //method is used in FertilizerGrowFlower script
    public bool Fertilise()
    {
        //ensure state is in planted
        if (growthState != FlowerGrowthState.Planted)
        {
            Debug.LogWarning("[POT] Fertilise failed — wrong state.");
            return false;
        }

        if (spriteRenderer != null && pollenPotSprite != null)
        {
            spriteRenderer.sprite = emptyPotSprite;
        }

        // Enable fertiliser + scissors colliders
        if (fertiliserCollider != null)
        {
            fertiliserCollider.enabled = true;
        }

        //new
        if (hybridFlower1 != null) hybridFlower1.SetActive(false);
        if (hybridFlower2 != null) hybridFlower2.SetActive(false);

        if (plantedHybrid == hybrid1Data)
        {
            hybridFlower1.SetActive(true);

            hybridFlower1.transform.SetParent(transform);
            hybridFlower1.transform.localPosition = new Vector3(0f, 3.2f, 0f);

            Debug.Log("Flower 1 parented");
        }
        else if (plantedHybrid == hybrid2Data)
        {
            hybridFlower2.SetActive(true);

            hybridFlower2.transform.SetParent(transform);
            hybridFlower2.transform.localPosition = new Vector3(0f, 3.2f, 0f);

            Debug.Log("Flower 2 parented");
        }

        //use when final flower is out
        //if (spriteRenderer != null && pollenPotSprite != null)
        //{
        //    spriteRenderer.sprite = potWithStudSprite;
        //}

        //changes the state to fertilised
        growthState = FlowerGrowthState.Fertilised;
        Debug.Log("[FERTILISE] Fertilizer applied");


        //add anim here, then grow method
        Grow();
        //if fertilise is successful
        return true;
    }

    public void Grow()
    {
        //ensure state is in fertilised
        if (growthState != FlowerGrowthState.Fertilised)
        {
            return;
        }
        //changes the state to grown
        growthState = FlowerGrowthState.Grown;
        Debug.Log("[GROW] Flower fully grown");

        // Enable fertiliser + scissors colliders
        //if (fertiliserCollider != null)
        //{
        //    fertiliserCollider.enabled = true;
        //} 

        if (scissorsCollider != null)
        {
            scissorsCollider.enabled = true;
        }

        // Optional popup
        if (toolsReadyPopup != null)
        {
            toolsReadyPopup.SetActive(true);
        }

        if (hybridFlowerManager != null)
        {
            //Notifies manager with hybrid data only
            hybridFlowerManager.OnHybridReadyToCut(plantedHybrid);
        }
    }

    // Add Code so that when done the fertilizer and scissors can use maybe a ui Pop up 
    public bool IsReadyToFertilise()
    {
        return growthState == FlowerGrowthState.Planted;
    }

    public void DisposeContents()
    {
        if (growthState == FlowerGrowthState.Empty)
            return;

        Debug.Log("[POT] Disposing contents");

        if (hybridFlower1 != null)
        {
            hybridFlower1.transform.SetParent(null);
            hybridFlower1.SetActive(false);
        }

        if (hybridFlower2 != null)
        {
            hybridFlower2.transform.SetParent(null);
            hybridFlower2.SetActive(false);
        }

        ResetPot();
    }

    void OnMouseDown()
    {
        BeeController bee = FindObjectOfType<BeeController>();
        bee.MoveToPot(this);
    }
}
