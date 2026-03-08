using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowerCutSwap : MonoBehaviour
{
    public ItemsSOScript flowerItemData;

    [Header("Things we spawn")]
    public GameObject cutFlowerPrefab;   // The flower top that appears after cutting

    [Header("UI")]
    public GameObject hybridFlowerCollectPopup;     // The "Flower Collected!" popup

    [Header("Scene")]
    public string sceneName;             // The place we go when player presses Home

    [Header("Pictures")]
    public Sprite cutFlowerSprite;       // Picture for the cut flower
 

    SpriteRenderer sr;   // Lets us change the flower picture
    bool cutDone;        // So we don't cut again and again

    void Awake()
    {
        // Get the flower picture component
        sr = GetComponent<SpriteRenderer>();

        // Hide the popup at the start
        if (hybridFlowerCollectPopup != null)
             hybridFlowerCollectPopup.SetActive(false);
    }

    // ?? THIS is the button function (Home button will call this)
    //public void LoadScene()
    //{
    //    // If we forgot to type the scene name, show error
    //    if (string.IsNullOrEmpty(sceneName))
    //    {
    //        Debug.LogError("No scene name typed! Put the scene name in Inspector.");
    //        return;
    //    }

    //    // Go to the new scene
    //    SceneManager.LoadScene(sceneName);
    //}

    // ?? This runs when we CUT the flower
    public void Cut()
    {
        // Stop if already cut
        if (cutDone) return;
        cutDone = true;

        // Hide the original flower
        if (sr != null)
            sr.enabled = false;

        // ?? Create the CUT flower (top part)
        if (cutFlowerPrefab != null)
        {
            var cutFlower = Instantiate(cutFlowerPrefab, transform.position, Quaternion.identity);

            // Change its picture
            var fsr = cutFlower.GetComponent<SpriteRenderer>();
            if (fsr != null && cutFlowerSprite != null)
                fsr.sprite = cutFlowerSprite;

            // Make sure we can CLICK the flower
            if (cutFlower.GetComponent<Collider2D>() == null)
                cutFlower.AddComponent<BoxCollider2D>();

            // When player clicks flower ? show popup + remove flower
            var clickHandler = cutFlower.AddComponent<CutFlowerClick_Internal>();
            clickHandler.hybridFlowerCollectPopup = hybridFlowerCollectPopup;
            clickHandler.hybridData = flowerItemData;
        }
    }

    // ?? This handles clicking the CUT flower
    private class CutFlowerClick_Internal : MonoBehaviour
    {
        public GameObject hybridFlowerCollectPopup;
        public ItemsSOScript hybridData;

        void OnMouseDown()
        {
            if (hybridFlowerCollectPopup != null)
            {
                hybridFlowerCollectPopup.SetActive(true);
            }
                

            // use the data that was passed in
            InventoryManager.Instance.AddHybrid(hybridData);

            //maybe not destroy
            //set inactive then reset??
            Destroy(gameObject);
        }
    }
}