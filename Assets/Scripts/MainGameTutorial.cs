using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainGameTutorial : MonoBehaviour
{
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;

    public GameObject next1;
    public GameObject next2;

    public GameObject black;
    public BoxCollider2D counterCollider;
    public BoxCollider2D hybridCollider;
    public Button exitButton;
    public GameObject counterButton;

    bool IsTutorial()
    {
        return PlayerPrefs.GetInt("TutorialDone", 0) == 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DisableAllTutorialUI();

        if (!IsTutorial()) return;

        DisableStationCollider();
        Step1();
    }

    public void DisableStationCollider()
    {
        counterCollider.enabled = false;
        hybridCollider.enabled = false;
    }

    public void EnableStationCollider()
    {
        counterCollider.enabled = true;
        hybridCollider.enabled = true;
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
        counterCollider.enabled = true;
        exitButton.enabled = false;
        
    }

    void Update()
    {
        if (counterButton.activeSelf == true && PlayerPrefs.GetInt("TutorialDone", 0) == 0)
        {
            Step4();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.DeleteKey("TutorialDone");
            Debug.Log("Tutorial reset!");
        }
    }

    public void Step4()
    {
        step3.SetActive(false);
        step4.SetActive(true);
    }

    public void EndTutorial()
    {
        DisableAllTutorialUI();
        PlayerPrefs.SetInt("TutorialDone", 1);
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
    }
}
