using UnityEngine;

public class HybridFlowerHover : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;

    [Header("Optional")]
    public GameObject soil; // if your hybrid has soil object (can leave null)

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        // set default sprite
        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnMouseEnter()
    {
        if (sr != null && hoverSprite != null)
        {
            sr.sprite = hoverSprite;

            if (soil != null)
                soil.SetActive(false);
        }
    }

    void OnMouseExit()
    {
        if (sr != null && normalSprite != null)
        {
            sr.sprite = normalSprite;

            if (soil != null)
                soil.SetActive(true);
        }
    }
}