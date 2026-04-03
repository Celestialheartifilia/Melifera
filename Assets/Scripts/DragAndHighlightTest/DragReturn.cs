using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DragReturn : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    SpriteRenderer sr;

    Vector2 offset;
    Vector2 startPos;

    bool dragging;

    public Bin bin;
    public PackingBin packingBin;

    public bool returnToStartPosition = true;

    private Animator scissorAnimator;

    [Header("Hover Sprite")]
    public Sprite normalSprite;
    public Sprite hoverSprite;

    [Header("SFX")]
    public bool playScissorsSFX = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();

        scissorAnimator = GetComponent<Animator>();

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        startPos = rb.position;
        rb.linearVelocity = Vector2.zero;

        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnMouseEnter()
    {
        if (!dragging && scissorAnimator != null)
            scissorAnimator.SetBool("isHover", true);

        if (sr != null && hoverSprite != null)
            sr.sprite = hoverSprite;
    }

    void OnMouseExit()
    {
        if (!dragging && scissorAnimator != null)
            scissorAnimator.SetBool("isHover", false);

        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void OnMouseDown()
    {
        dragging = true;

        if (scissorAnimator != null)
        {
            scissorAnimator.SetBool("isHover", false);
            scissorAnimator.SetBool("isCutting", true);
        }

        if (playScissorsSFX && SoundEffectPlayer.Instance != null)
        {
            SoundEffectPlayer.Instance.PlaySound(
                SoundEffectPlayer.Instance.scissorsCuttingSFX
            );
        }

        if (sr != null && hoverSprite != null)
            sr.sprite = hoverSprite;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = rb.position - mouseWorld;
    }

    void OnMouseUp()
    {
        dragging = false;

        if (scissorAnimator != null)
            scissorAnimator.SetBool("isCutting", false);

        if (bin != null)
            bin.TryDispose();

        if (packingBin != null)
            packingBin.TryDispose();

        rb.linearVelocity = Vector2.zero;
        rb.position = startPos;

        if (sr != null && normalSprite != null)
            sr.sprite = normalSprite;
    }

    void FixedUpdate()
    {
        if (!dragging) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = mouseWorld + offset;

        rb.MovePosition(newPos);
    }
}