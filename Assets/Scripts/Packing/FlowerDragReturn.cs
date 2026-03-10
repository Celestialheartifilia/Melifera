using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DragFlower : MonoBehaviour
{
    Camera cam;
    Vector2 offset;
    bool dragging;

    Vector3 homePosition;
    Quaternion homeRotation;

    public DragLeaf[] leaves;
    public PackingBin packingBin;

    void Awake()
    {
        cam = Camera.main;

        // default home = scene position
        homePosition = transform.position;
        homeRotation = transform.rotation;
    }

    void OnMouseDown()
    {
        dragging = true;

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mouse;

        // disable leaf dragging while flower moves
        foreach (var leaf in leaves)
        {
            if (leaf != null)
                leaf.enabled = false;
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

        foreach (var leaf in leaves)
        {
            if (leaf != null)
                leaf.enabled = true;
        }

        // return to current bouquet/home transform
        transform.position = homePosition;
        transform.rotation = homeRotation;
    }

    public void SetHomeTransform(Vector3 newPosition, Quaternion newRotation)
    {
        homePosition = newPosition;
        homeRotation = newRotation;
    }

    public void ResetFlower()
    {
        transform.position = homePosition;
        transform.rotation = homeRotation;
    }
}
