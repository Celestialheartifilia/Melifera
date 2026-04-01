using UnityEngine;

public class NormalFlower : MonoBehaviour
{
    public ItemsSOScript flowerData;
    public bool isPollinated;

    [Header("Pollination Indicator")]
    public GameObject pollinateIndicator;

    [Header("Hover Sprite")]
    public Sprite normalSprite;
    public Sprite hoverSprite;
    public GameObject soil;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (pollinateIndicator != null)
            pollinateIndicator.SetActive(false);

        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    public void SetPollinated(bool value)
    {
        isPollinated = value;

        // hide indicator after pollinated
        if (pollinateIndicator != null)
            pollinateIndicator.SetActive(false);
    }

    // controlled by BeeController
    public void ShowPollinateIndicator(bool show)
    {
        if (pollinateIndicator != null)
            pollinateIndicator.SetActive(show);
    }

    // =========================
    // HOVER
    // =========================
    void OnMouseEnter()
    {
        if (sr != null && hoverSprite != null)
        {
            sr.sprite = hoverSprite;
            if (soil != null) soil.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        if (sr != null && normalSprite != null)
        {
            sr.sprite = normalSprite;
            if (soil != null) soil.SetActive(true);
        }
    }

    void OnMouseDown()
    {
        BeeController bee = FindObjectOfType<BeeController>();

        if (bee == null) return;

        if (!bee.canMoveToFlowers)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("Finish the current hybrid first.");
            return;
        }

        bee.MoveToFlower(this);
    }
}