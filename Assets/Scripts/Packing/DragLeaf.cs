using UnityEngine;

public class DragLeaf : MonoBehaviour
{
    Camera cam;
    Vector2 offset;
    bool dragging;

    Transform originalParent;
    Vector2 startLocalPos;

    bool detached = false;

    public DragFlower flower;

    void Awake()
    {
        cam = Camera.main;
        originalParent = transform.parent;
        startLocalPos = transform.localPosition;
    }

    void OnMouseDown()
    {
        dragging = true;

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mouse;

        // disable flower dragging
        flower.enabled = false;

        // detach from flower
        if (!detached)
        {
            transform.parent = null;
            detached = true;
        }
    }

    void OnMouseDrag()
    {
        if (!dragging) return;

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mouse + offset;
    }

    void OnMouseUp()
    {
        dragging = false;

        // enable flower again
        flower.enabled = true;
    }

    public void ResetLeaf()
    {
        detached = false;
        transform.parent = originalParent;
        transform.localPosition = startLocalPos;
    }
}
