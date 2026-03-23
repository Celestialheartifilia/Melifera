using UnityEngine;
using UnityEngine.UI;

public class HybridTutorial : MonoBehaviour
{
    public PollinationManager pollinationManager;
    bool hybridReadyTriggered = false;
    public Pot hybridpot;
    bool plantedTriggered = false;
    bool fertilisedTriggered = false;
    bool cutTriggered = false;
    bool collectedTriggered = false;

    [Header("Steps")]
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;
    public GameObject step5;
    public GameObject step6;
    public GameObject step7;
    public GameObject step8;
    public GameObject step9;
    public GameObject step10;
    public GameObject step11;

    [Header("Next")]
    public GameObject next1;
    public GameObject next2;
    public GameObject next3;
    public GameObject next4;
    public GameObject next5;

    [Header("Overlay")]
    public GameObject black;
    public GameObject black2;
    public GameObject black3;

    [Header("Arrow")]
    public GameObject arrow1;
    public GameObject arrow2;

    [Header("Buttons / UI")]
    public BeeController beeController;
    public GameObject orderButton;
    public GameObject hybridBookButton;
    public GameObject pot;
    public GameObject fertiliser;
    public GameObject scissors;
    public GameObject flower;
    public Button exitButton;
    public Button collectedContinueButton;

    //wrong hybrid
    ItemsSOScript requiredHybrid;
    public GameObject wrongHybridPopup;
    bool waitingForClear = false;

    int step = 0;

    bool IsTutorial()
    {
        return PlayerPrefs.GetInt("HybridTutorialDone", 0) == 0;
    }

    void Start()
    {

        if (OrderTakingManager.Instance != null && OrderTakingManager.Instance.currentOrder != null)
        {
            foreach (var item in OrderTakingManager.Instance.currentOrder.orderedItems)
            {
                if (OrderTakingManager.Instance.hybridFlowerItems.Contains(item))
                {
                    requiredHybrid = item;
                    break;
                }
            }
        }

        //orderButton = GameObject.FindWithTag("OrderViewButton");
        //hybridBookButton = GameObject.FindWithTag("HybridGuideButton");

        Debug.Log("Tutorial Start Running");

        DisableAll();

        if (!IsTutorial())
        {
            Debug.Log("Tutorial skipped");
            return;
        }

        Debug.Log("Starting Step 1");

        step = 1;
        ShowStep();

    }

    void Update()
    {
        if (!IsTutorial()) return;

        // DEBUG skip
        if (Input.GetKeyDown(KeyCode.H))
        {
            NextStep();
        }

        if (step == 6 && pollinationManager.ReadyHybrid != null)
        {
            // correct hybrid
            if (pollinationManager.ReadyHybrid == requiredHybrid)
            {
                hybridReadyTriggered = true;
                NextStep();
            }
            else
            {
                ShowWrongHybridPopup();
            }
        }

        if (!plantedTriggered && step == 7 && hybridpot.growthState == Pot.FlowerGrowthState.Planted)
        {
            plantedTriggered = true;
            NextStep();
        }

        // STEP 9
        if (!fertilisedTriggered && step == 8 && (hybridpot.growthState == Pot.FlowerGrowthState.Fertilised || hybridpot.growthState == Pot.FlowerGrowthState.Grown))
        {
            fertilisedTriggered = true;
            NextStep();
        }

        // STEP 10
        if (!cutTriggered && step == 9)
        {
            FlowerCutSwap[] flowers = FindObjectsByType<FlowerCutSwap>(FindObjectsSortMode.None);

            foreach (var f in flowers)
            {
                if (f.cutDone)
                {
                    cutTriggered = true;
                    NextStep();
                    break;
                }
            }
        }

        // STEP 11
        if (!collectedTriggered && step == 10 && cutTriggered)
        {
            GameObject cutFlower = GameObject.FindWithTag("CutFlower");

            if (cutFlower == null)
            {
                collectedTriggered = true;
                NextStep();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerPrefs.DeleteKey("HybridTutorialDone");
            Debug.Log("Tutorial reset!");
        }
    }

    // ========================
    // STEP CONTROL
    // ========================

    public void NextStep()
    {
        step++;
        ShowStep();
    }

    void ShowStep()
    {
        DisableAll();

        switch (step)
        {
            case 1:
                // Explain hybrid station
                step1.SetActive(true);
                next1.SetActive(true);
                black.SetActive(true);
                beeController.enabled = false;
                exitButton.enabled = false;
                pot.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
               
                break;

            case 2:
                // Clear pollination button
                step2.SetActive(true);
                next2.SetActive(true);
                black2.SetActive(true);
                break;

            case 3:
                // Bin explanation
                step3.SetActive(true);
                next3.SetActive(true);
                black3.SetActive(true);
                break;

            case 4:
                // Order list button
                black3.SetActive(true);
                step4.SetActive(true);
                next4.SetActive(true);
                arrow1.SetActive(true);
                break;

            case 5:
                // Hybrid book open/close
                black3.SetActive(true);
                step5.SetActive(true);
                next5.SetActive(true);
                arrow2.SetActive(true);
                break;

            case 6:
                // select flowers
                step6.SetActive(true);
                beeController.enabled = true;
                break;

            case 7:
                // Plant in pot
                step7.SetActive(true);
                break;

            case 8:
                // Fertilise
                step8.SetActive(true);
                break;

            case 9:
                // Cut flower
                step9.SetActive(true);
                break;

            case 10:
                // Collect flower
                step10.SetActive(true);
                beeController.enabled = false;
                break;

            case 11:
                //leave
                step11.SetActive(true);
                collectedContinueButton.enabled = false;
                break;

            default:
                EndTutorial();
                break;
        }
    }

    // ========================
    // END
    // ========================

    public void EndTutorial()
    {
        DisableAll();
        beeController.enabled = true;
        exitButton.enabled = true;
        collectedContinueButton.enabled = true;
        pot.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        PlayerPrefs.SetInt("HybridTutorialDone", 1);
    }

    void DisableAll()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);
        step5.SetActive(false);
        step6.SetActive(false);
        step7.SetActive(false);
        step8.SetActive(false);
        step9.SetActive(false);
        step10.SetActive(false);
        step11.SetActive(false);

        black.SetActive(false);
        black2.SetActive(false);
        black3.SetActive(false);

        next1.SetActive(false);
        next2.SetActive(false);
        next3.SetActive(false);
        next4.SetActive(false);
        next5.SetActive(false);

        arrow1.SetActive(false);
        arrow2.SetActive(false);
    }

    void ShowWrongHybridPopup()
    {
        if (waitingForClear) return;

        wrongHybridPopup.SetActive(true);
        waitingForClear = true;

        beeController.enabled = false; // optional lock
    }

    public void OnClearPollinationPressed()
    {
        if (!waitingForClear) return;

        wrongHybridPopup.SetActive(false);
        waitingForClear = false;

        beeController.enabled = true;

        pollinationManager.ResetPollination();
    }
}
