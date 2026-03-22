using UnityEngine;
using UnityEngine.SceneManagement;

public class TopNavUI : MonoBehaviour
{
    public static TopNavUI instance;

    [Header("UI Objects")]
    public GameObject orderViewButton;
    public GameObject hybridGuideButton;
    public GameObject orderView;
    public GameObject hybridGuide;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        UpdateUI(SceneManager.GetActiveScene().name);
        orderView.SetActive(true);
        SpriteRenderer sr = orderView.GetComponent<SpriteRenderer>();
        if (sr.enabled == false)
        {
            sr.enabled = true;
        }
        sr.enabled = true;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUI(scene.name);
    }

    void UpdateUI(string sceneName)
    {
        // Hide everything first
        orderViewButton.SetActive(false);
        hybridGuideButton.SetActive(false);
        orderView.SetActive(false);
        orderViewButton.SetActive(false);

        if (sceneName == "HybridingFlowerScene")
        {
            orderViewButton.SetActive(true);
            hybridGuideButton.SetActive(true);
        }
        else if (sceneName == "PackingScene")
        {
            orderViewButton.SetActive(true);
        }
        else if (sceneName == "MainGameScene" || sceneName == "OrderTakingScene")
        {
            orderView.SetActive(false);
            hybridGuide.SetActive(false);
        }

        //orderViewButton.SetActive(true);
        //hybridGuideButton.SetActive(true);
        //orderView.SetActive(true);
        //orderViewButton.SetActive(true);
    }
}
