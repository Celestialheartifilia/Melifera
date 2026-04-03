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

        // leaf pluck SFX
        if (SoundEffectPlayer.Instance != null)
            SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.pluckOutLeavesSFX);

        Vector2 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
        offset = (Vector2)transform.position - mouse;

        flower.enabled = false;

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
        flower.enabled = true;
    }

    public void ResetLeaf()
    {
        detached = false;
        transform.parent = originalParent;
        transform.localPosition = startLocalPos;
    }
}