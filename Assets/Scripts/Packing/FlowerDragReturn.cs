using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragFlower : MonoBehaviour
{
    Camera cam;
    Vector2 offset;
    bool dragging;

    Vector2 startPos;

    public DragLeaf[] leaves;
    public PackingBin packingBin;

    void Awake()
    {
        cam = Camera.main;
        startPos = transform.position;
    }

    void OnMouseDown()
    {
        dragging = true;

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mouse;

        // disable leaf dragging while flower moves
        foreach (var leaf in leaves)
            leaf.enabled = false;
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

        // enable leaves again
        foreach (var leaf in leaves)
            leaf.enabled = true;

        transform.position = startPos;

        //if (packingBin == null || !packingBin.IsFlowerInside(this))
        //{
        //    transform.position = startPos;
        //}
    }

    public void ResetFlower()
    {
        transform.position = startPos;
    }
}
