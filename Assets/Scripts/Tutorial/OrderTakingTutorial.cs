using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class OrderTakingTutorial : MonoBehaviour
{
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;

    public GameObject next1;

    public GameObject black;
    public GameObject black2;

    public Button takeOrderButton;
    public GameObject orderBubble;

    public GameObject arrow1;
    public GameObject arrow2;

    bool step3active = false;

    bool IsTutorial()
    {
        return PlayerPrefs.GetInt("OrderTutorialDone", 0) == 0;
    }

    void Start()
    {
        DisableAllTutorialUI();

        if (!IsTutorial()) return;

        Step1();
    }

    //STEP 1: click take order
    public void Step1()
    {
        black.SetActive(true);
        step1.SetActive(true);
        arrow1.SetActive(true);
    }

    void Update()
    {
        if (!IsTutorial()) return;

        // when order bubble appears, move to Step2
        if (orderBubble.activeSelf == true && PlayerPrefs.GetInt("OrderTutorialDone", 0) == 0 && !step3active)
        {
            Step2();
        }
    }

    //STEP 2: show order explanation
    public void Step2()
    {
        step1.SetActive(false);
        black.SetActive(false);
        arrow1.SetActive(false);

        step2.SetActive(true);
        next1.SetActive(true);
    }

    public void Step3()
    {
        step3active = true;
        step2.SetActive(false);
        next1.SetActive(false);
        step3.SetActive(true);
        black2.SetActive(true);
        arrow2.SetActive(true);

    }

    public void EndTutorial()
    {
        DisableAllTutorialUI();
        PlayerPrefs.SetInt("OrderTutorialDone", 1);
    }

    public void DisableAllTutorialUI()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);

        next1.SetActive(false);
        black.SetActive(false);
        black2.SetActive(false);

        arrow1.SetActive(false);
        arrow2.SetActive(false);
    }
}
