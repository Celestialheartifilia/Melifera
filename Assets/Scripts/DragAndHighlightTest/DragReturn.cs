using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class DragReturn : MonoBehaviour
{
    Rigidbody2D rb; 
    Camera cam;

    Vector2 offset; 
    Vector2 startPos;

    bool dragging;

    public Bin bin; 
    public PackingBin packingBin;

    public bool returnToStartPosition = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        cam = Camera.main;

        rb.gravityScale = 0f; // ensure no gravity
        rb.linearVelocity = Vector2.zero;
    }

    // IMPORTANT: store correct start position every time object becomes active
    void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        startPos = rb.position;
        rb.linearVelocity = Vector2.zero;
    }

    void OnMouseDown()
    {
        dragging = true;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = rb.position - mouseWorld;
    }

    void OnMouseUp()
    {
        dragging = false;

        // Call bin disposal BEFORE resetting position
        if (bin != null)
            bin.TryDispose();

        if (packingBin != null)
            packingBin.TryDispose();

        // Hard reset physics properly
        rb.linearVelocity = Vector2.zero;
        rb.position = startPos;
    }

    void FixedUpdate()
    {
        if (!dragging) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newPos = mouseWorld + offset;

        rb.MovePosition(newPos);
    }

}