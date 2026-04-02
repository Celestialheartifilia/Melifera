using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LeafHover : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite hoverSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnMouseEnter()
    {
        if (sr != null && hoverSprite != null)
            sr.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnDisable()
    {
        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }
}