using UnityEngine;
using UnityEngine.UI;

public class HybridTutorial : MonoBehaviour
{
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
    public GameObject step12;

    public GameObject next1;
    public GameObject next2;
    public GameObject next3;

    [Header("Overlay")]
    public GameObject black;
    public GameObject black2;
    public GameObject black3;

    [Header("Buttons / UI")]
    public BeeController beeController;
    public GameObject orderButton;
    public GameObject hybridBookButton;
    public GameObject pot;
    public GameObject fertiliser;
    public GameObject scissors;
    public GameObject flower;

    int step = 0;

    bool IsTutorial()
    {
        return PlayerPrefs.GetInt("HybridTutorialDone", 0) == 0;
    }

    void Start()
    {
        orderButton = GameObject.FindWithTag("OrderViewButton");
        hybridBookButton = GameObject.FindWithTag("HybridGuideButton");

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
                black.SetActive(true);
                step4.SetActive(true);
                break;

            case 5:
                // Hybrid book open
                black.SetActive(true);
                step5.SetActive(true);
                break;

            case 6:
                // Close hybrid book
                step6.SetActive(true);
                break;

            case 7:
                // Select flowers
                step7.SetActive(true);
                break;

            case 8:
                // Plant in pot
                black.SetActive(true);
                step8.SetActive(true);
                break;

            case 9:
                // Fertilise
                step9.SetActive(true);
                break;

            case 10:
                // Cut flower
                step10.SetActive(true);
                break;

            case 11:
                // Collect flower
                black.SetActive(true);
                step11.SetActive(true);
                break;

            case 12:
                // Final message, go packing
                step12.SetActive(true);
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
        PlayerPrefs.SetInt("HybridTutorialDone", 1);
    }

    void DisableAll()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        //step4.SetActive(false);
        //step5.SetActive(false);
        //step6.SetActive(false);
        //step7.SetActive(false);
        //step8.SetActive(false);
        //step9.SetActive(false);
        //step10.SetActive(false);
        //step11.SetActive(false);
        //step12.SetActive(false);

        black.SetActive(false);
        black2.SetActive(false);
        black3.SetActive(false);

        next1.SetActive(false);
        next2.SetActive(false);
    }
}
