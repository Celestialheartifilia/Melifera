using UnityEngine;
using UnityEngine.UI;

public class MainGameTutorial : MonoBehaviour
{
    [Header("Start")]
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;

    public GameObject next1;
    public GameObject next2;

    [Header("Arrows")]
    public GameObject arrow1C;
    public GameObject arrow2C;
    public GameObject arrow1H;
    public GameObject arrow2H;
    public GameObject arrow1P;
    public GameObject arrow2P;

    public GameObject black;
    public BoxCollider2D counterCollider;
    public BoxCollider2D hybridCollider;
    public Button exitButton;
    public GameObject counterButton;

    [Header("Hybrid")]
    public GameObject step1hybrid;
    public GameObject step2hybrid;
    public GameObject hybridButton;
    public BoxCollider2D packingCollider;

    [Header("Packing")]
    public GameObject step1packing;
    public GameObject step2packing;
    public GameObject packingButton;

    bool IsTutorial()
    {
        return PlayerPrefs.GetInt("MainTutorialDone", 0) == 0;
    }


    bool IsFirstCustomerGuide()
    {
        return OrderTakingManager.Instance != null &&
               OrderTakingManager.Instance.currentCustomerIndex == 1 &&
               PlayerPrefs.GetInt("GoHybridTutorialDone", 0) == 0;
    }

    bool IsPackingGuide()
    {
        return OrderTakingManager.Instance != null &&
               OrderTakingManager.Instance.currentCustomerIndex == 1 &&
               PlayerPrefs.GetInt("GoHybridTutorialDone", 0) == 1 && // hybrid finished
               PlayerPrefs.GetInt("GoPackingTutorialDone", 0) == 0;
    }

    bool allTutorialCompleted()
    {
        return PlayerPrefs.GetInt("GoPackingTutorialDone", 0) == 1 && PlayerPrefs.GetInt("GoHybridTutorialDone", 0) == 1 && PlayerPrefs.GetInt("MainTutorialDone", 0) == 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        DisableAllTutorialUI();

        if (IsTutorial())
        {
            DisableStationCollider();
            Step1();
            return;
        }

        if (IsFirstCustomerGuide())
        {
            Step1Hybrid();
            return;
        }

        if (IsPackingGuide())
        {
            Step1Packing();
            return;
        }

        if (allTutorialCompleted())
        {
            EnableStationCollider();
            counterButton.GetComponent<PolygonCollider2D>().enabled = true;
            hybridButton.GetComponent<PolygonCollider2D>().enabled = true;
            packingButton.GetComponent<PolygonCollider2D>().enabled = true;
        }
    }

    public void DisableStationCollider()
    {
        counterCollider.enabled = false;
        hybridCollider.enabled = false;
        packingCollider.enabled = false;
    }

    public void EnableStationCollider()
    {
        counterCollider.enabled = true;
        hybridCollider.enabled = true;
        packingCollider.enabled = true;
    }

    public void Step1()
    {
        black.SetActive(true);
        step1.SetActive(true);
        next1.SetActive(true);
    }

    public void Step2()
    {
        step1.SetActive(false);
        next1.SetActive(false);
        step2.SetActive(true);
        next2.SetActive(true);
    }
    public void Step3()
    {
        step2.SetActive(false);
        next2.SetActive(false);
        black.SetActive(false);
        step3.SetActive(true);
        arrow1C.SetActive(true);
        counterCollider.enabled = true;
        exitButton.enabled = false;

    }

    void Update()
    {
        if (counterButton.activeSelf == true && PlayerPrefs.GetInt("MainTutorialDone", 0) == 0)
        {
            Step4();
        }

        if (hybridButton.activeSelf == true && IsFirstCustomerGuide())
        {
            Step2Hybrid();
        }

        if (packingButton.activeSelf == true && IsPackingGuide())
        {
            Step2Packing();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey("MainTutorialDone");
            PlayerPrefs.DeleteKey("OrderTutorialDone");
            PlayerPrefs.DeleteKey("GoHybridTutorialDone");
            PlayerPrefs.DeleteKey("HybridTutorialDone");
            PlayerPrefs.DeleteKey("GoPackingTutorialDone");
            Debug.Log("Tutorial reset!");
        }
    }

    public void Step4()
    {
        step3.SetActive(false);
        arrow1C.SetActive(false);
        step4.SetActive(true);
        arrow2C.SetActive(true);
    }

    public void EndTutorial()
    {
        DisableAllTutorialUI();
        PlayerPrefs.SetInt("MainTutorialDone", 1);
    }

    public void DisableAllTutorialUI()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);

        next1.SetActive(false);
        next2.SetActive(false);

        black.SetActive(false);

        step1hybrid.SetActive(false);
        step2hybrid.SetActive(false);

        arrow1C.SetActive(false);
        arrow2C.SetActive(false);
        arrow1H.SetActive(false);
        arrow2H.SetActive(false);
        arrow1P.SetActive(false);
        arrow2P.SetActive(false);

        step1packing.SetActive(false);
        step2packing.SetActive(false);

    }

    //hybrid
    public void Step1Hybrid()
    {
        step1hybrid.SetActive(true);
        arrow1H.SetActive(true);

        // lock everything except hybrid
        counterCollider.enabled = false;
        packingCollider.enabled = false;
        exitButton.enabled = false;

        counterButton.GetComponent<PolygonCollider2D>().enabled = false;
        hybridCollider.enabled = true;

    }

    public void Step2Hybrid()
    {
        step1hybrid.SetActive(false);
        arrow1H.SetActive(false);
        step2hybrid.SetActive(true);
        arrow2H.SetActive(true);

    }

    public void EndTutorialHybrid()
    {
        DisableAllTutorialUI();
        PlayerPrefs.SetInt("GoHybridTutorialDone", 1);
    }

    //packing
    public void Step1Packing()
    {
        step1packing.SetActive(true);
        arrow1P.SetActive(true);

        // lock everything except hybrid
        counterCollider.enabled = false;
        hybridCollider.enabled = false;
        exitButton.enabled = false;

        hybridButton.GetComponent<PolygonCollider2D>().enabled = false;
        packingCollider.enabled = true;

    }

    public void Step2Packing()
    {
        step1packing.SetActive(false);
        arrow1P.SetActive(false);
        step2packing.SetActive(true);
        arrow2P.SetActive(true);

    }

    public void EndTutorialPacking()
    {
        DisableAllTutorialUI();
        EnableStationCollider();
        PlayerPrefs.SetInt("GoPackingTutorialDone", 1);

    }
}

