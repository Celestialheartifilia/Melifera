using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 8f;
    public Camera cam;

    [Header("Movement")]
    public float moveThreshold = 2f; // how big the mouse movement must be

    [Header("Direction animation objects")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (cam == null)
            cam = Camera.main;

        ShowOnly(frontObj);
    }

    void FixedUpdate()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 world = cam.ScreenToWorldPoint(mouse);

        Vector2 targetPos = new Vector2(world.x, world.y);
        Vector2 current = rb.position;

        Vector2 dir = targetPos - current;

        // ONLY move if distance is big enough
        if (dir.magnitude < moveThreshold)
            return;

        Vector2 newPos = Vector2.MoveTowards(current, targetPos, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Direction animation
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if (dir.x > 0) ShowOnly(rightObj);
            else ShowOnly(leftObj);
        }
        else
        {
            if (dir.y > 0) ShowOnly(backObj);
            else ShowOnly(frontObj);
        }
    }

    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);
    }
}