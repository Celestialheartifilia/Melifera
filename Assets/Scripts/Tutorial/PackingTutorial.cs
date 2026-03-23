using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PackingTutorial : MonoBehaviour
{
    public PackingManager packingManager;

    [Header("Steps")]
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;
    public GameObject step5;
    public GameObject step6;

    [Header("Next")]
    public GameObject next1;

    [Header("Arrows")]
    public GameObject arrow1;
    public GameObject arrow2;
    public GameObject arrow3;
    public GameObject arrow4;
    public GameObject arrow5;

    [Header("Overlay")]
    public GameObject black;

    [Header("UI")]
    public Button orderCompleteButton;

    [Header("Wrong Flower")]
    public GameObject wrongFlowerPopup;
    bool waitingForClear = false;

    ItemsSOScript requiredFlower;

    bool flowerSpawned = false;
    bool leavesDone = false;
    bool wrapDone = false;
    bool orderPressed = false;
    bool resultShown = false;

    int step = 0;

    bool IsTutorial()
    {
        return PlayerPrefs.GetInt("PackingTutorialDone", 0) == 0;
    }

    void Start()
    {
        DisableAll();

        if (!IsTutorial()) return;

        // get required flower from order
        if (OrderTakingManager.Instance != null && OrderTakingManager.Instance.currentOrder != null)
        {
            foreach (var item in OrderTakingManager.Instance.currentOrder.orderedItems)
            {
                if (OrderTakingManager.Instance.hybridFlowerItems.Contains(item))
                {
                    requiredFlower = item;
                    break;
                }
            }
        }

        step = 1;
        ShowStep();
    }

    void Update()
    {
        if (!IsTutorial()) return;

        // STEP 2, detect flower spawn
        if (!flowerSpawned && step == 2)
        {
            if (packingManager != null && OrderTakingManager.Instance.currentBouquet.flowers.Count > 0)
            {
                ItemsSOScript spawned = OrderTakingManager.Instance.currentBouquet.flowers[0];

                if (spawned == requiredFlower)
                {
                    flowerSpawned = true;
                    NextStep();
                }
                else
                {
                    ShowWrongFlowerPopup();
                }
            }
        }

        // STEP 3, leaves removed
        if (!leavesDone && step == 3)
        {
            if (!packingManager.pluckingInProgress) // leaves done
            {
                leavesDone = true;
                NextStep();
            }
        }

        // STEP 4, wrap + accessory
        if (!wrapDone && step == 4)
        {
            if (packingManager != null &&
                OrderTakingManager.Instance.currentBouquet.wrap != null &&
                OrderTakingManager.Instance.currentBouquet.accessory != null)
            {
                wrapDone = true;
                NextStep();
            }
        }

        // STEP 5, order complete pressed
        if (!orderPressed && step == 5 && orderCompleteButton != null)
        {
            if (!orderCompleteButton.interactable)
            {
                orderPressed = true;
                NextStep();
            }
        }

        // STEP 6, result popup
        if (!resultShown && step == 6)
        {
            if (packingManager != null &&
                (packingManager.CorrectOrderPrompt.activeSelf ||
                 packingManager.WrongOrderPrompt.activeSelf))
            {
                resultShown = true;
                NextStep();
            }
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
                step1.SetActive(true);
                next1.SetActive(true);
                black.SetActive(true);
                break;

            case 2:
                step2.SetActive(true);
                arrow1.SetActive(true);
                break;

            case 3:
                step3.SetActive(true);
                arrow2.SetActive(true);
                break;

            case 4:
                step4.SetActive(true);
                arrow3.SetActive(true);
                break;

            case 5:
                step5.SetActive(true);
                arrow4.SetActive(true);
                break;

            case 6:
                step6.SetActive(true);
                break;

            default:
                EndTutorial();
                break;
        }
    }

    // ========================
    // WRONG FLOWER
    // ========================
    void ShowWrongFlowerPopup()
    {
        if (waitingForClear) return;

        wrongFlowerPopup.SetActive(true);
        arrow1.SetActive(true);
        waitingForClear = true;
    }

    public void OnClearFlowerPressed()
    {
        if (!waitingForClear) return;

        wrongFlowerPopup.SetActive(false);
        waitingForClear = false;

        packingManager.DisposeWholeBouquet();
    }

    // ========================
    // END
    // ========================
    public void EndTutorial()
    {
        DisableAll();
        PlayerPrefs.SetInt("PackingTutorialDone", 1);
        PlayerPrefs.Save();
    }

    void DisableAll()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);
        step5.SetActive(false);
        step6.SetActive(false);

        next1.SetActive(false);

        arrow1.SetActive(false);
        arrow2.SetActive(false);
        arrow3.SetActive(false);
        arrow4.SetActive(false);
        arrow5.SetActive(false);

        black.SetActive(false);
    }
}