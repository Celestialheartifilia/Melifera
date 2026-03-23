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

    //Vector3 originalPos = new Vector3(-966.6f, -538f, 0f);
    Vector3 packingPos = new Vector3(-3.1f, 3.1f, 0f);
    private Vector3 originalPos;

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
        originalPos = orderView.transform.position;
        Debug.Log("Original Pos: " + originalPos);
        UpdateUI(SceneManager.GetActiveScene().name);
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
            orderView.transform.position = originalPos;

        }
        else if (sceneName == "PackingScene")
        {
            orderViewButton.SetActive(true);
            orderView.transform.position = packingPos;
            Debug.Log(orderView.transform.position);
            Debug.Log(packingPos);
        }
        else if (sceneName == "MainGameScene" || sceneName == "OrderTakingScene")
        {
            orderView.SetActive(false);
            hybridGuide.SetActive(false);
        }
    }
}
