using UnityEngine;

public class NormalFlower : MonoBehaviour
{
    public ItemsSOScript flowerData;
    public bool isPollinated;

    [Header("Speech Bubble")]
    public GameObject speechIndicator;

    [Header("Hover Sprite")]
    public Sprite normalSprite;
    public Sprite hoverSprite;
    public GameObject soil;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (speechIndicator != null)
            speechIndicator.SetActive(false);

        // ensure default sprite is set
        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    public void SetPollinated(bool value)
    {
        isPollinated = value;
        // Later: change sprite / play effect here
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BeeController>() != null)
        {
            if (speechIndicator != null)
                speechIndicator.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<BeeController>() != null)
        {
            if (speechIndicator != null)
                speechIndicator.SetActive(false);
        }
    }

    // HOVER START
    void OnMouseEnter()
    {
        if (sr != null && hoverSprite != null)
        {
            sr.sprite = hoverSprite;
            soil.SetActive(false);
        }
            

    }

    // HOVER END
    void OnMouseExit()
    {
        if (sr != null && normalSprite != null)
        {
            sr.sprite = normalSprite;
            soil.SetActive(true);
        }
    }

    void OnMouseDown()
    {
        BeeController bee = FindObjectOfType<BeeController>();
        bee.MoveToFlower(this);
    }
}