using UnityEngine;

public class ResetTutorial : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.DeleteKey("MainTutorialDone");
        PlayerPrefs.DeleteKey("OrderTutorialDone");
        PlayerPrefs.DeleteKey("GoHybridTutorialDone");
        PlayerPrefs.DeleteKey("HybridTutorialDone");
        PlayerPrefs.DeleteKey("GoPackingTutorialDone");
        Debug.Log("Tutorial reset!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
