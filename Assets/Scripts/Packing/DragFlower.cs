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
        homePosition = transform.position;
        homeRotation = transform.rotation;
    }

    void OnMouseDown()
    {
        dragging = true;

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mouse;

        foreach (var leaf in leaves)
        {
            if (leaf == null) continue;

            leaf.enabled = false;

            Rigidbody2D rb = leaf.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Static;
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

        bool disposed = false;

        if (packingBin != null && packingBin.IsObjectInsideBin(gameObject))
        {
            packingBin.TryDisposeFlower(gameObject);
            disposed = true;
        }

        foreach (var leaf in leaves)
        {
            if (leaf == null) continue;

            leaf.enabled = true;

            Rigidbody2D rb = leaf.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Kinematic;
        }

        if (!disposed)
        {
            transform.position = homePosition;
            transform.rotation = homeRotation;
        }
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