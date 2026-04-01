using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public float speed = 8f;
    public float stopDistance = 0.05f;

    [Header("Direction animation objects")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;

    Rigidbody2D rb;

    Vector2 currentTarget;
    bool hasTarget = false;
    bool isMoving = false;

    public Animator beeLeftAnimator;
    public Animator beeRightAnimator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ShowOnly(leftObj);
        if (beeLeftAnimator != null)
            beeLeftAnimator.SetBool("Idle", true);
    }

    void Start()
    {
        if (StationManager.hasSpawn)
        {
            rb.position = StationManager.playerSpawnPosition;
            transform.position = StationManager.playerSpawnPosition;

            StationManager.hasSpawn = false; // reset after using
        }
    }
    void FixedUpdate()
    {
        if (!hasTarget) return;

        Vector2 current = rb.position;
        Vector2 dir = currentTarget - current;

        // Check if reached
        if (dir.magnitude <= stopDistance)
        {
            rb.MovePosition(currentTarget);
            hasTarget = false;
            isMoving = false;

            if (!isMoving)
            {
                if (beeLeftAnimator != null)
                    beeLeftAnimator.SetBool("Idle", true);

                if (beeRightAnimator != null)
                    beeRightAnimator.SetBool("Idle", true);
            }

            // Only show front/back when reached
            if (Mathf.Abs(dir.y) == Mathf.Abs(dir.x))
            {
                if (beeLeftAnimator != null)
                    beeLeftAnimator.SetBool("Idle", true);

                if (beeRightAnimator != null)
                    beeRightAnimator.SetBool("Idle", true);

                //ShowOnly(backObj);
                //if (dir.y > 0) ShowOnly(backObj);
                //else ShowOnly(frontObj);
            }

            return;
        }

        // Move
        Vector2 newPos = Vector2.MoveTowards(current, currentTarget, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
        isMoving = true;

        // While moving, only left/right
        if (Mathf.Abs(dir.x) > 0.1f)
        {
            if (dir.x > 0) ShowOnly(rightObj);
            else ShowOnly(leftObj);
        }
    }

    // Called by move to station
    public void SetTarget(Vector2 target)
    {
        currentTarget = target;
        hasTarget = true;
    }

    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);

        // Reset all animators first
        if (beeLeftAnimator != null)
            beeLeftAnimator.SetBool("Idle", false);

        if (beeRightAnimator != null)
            beeRightAnimator.SetBool("Idle", false);

        // Activate correct animator
        if (obj == leftObj && beeLeftAnimator != null)
            beeLeftAnimator.SetBool("Idle", false);

        if (obj == rightObj && beeRightAnimator != null)
            beeRightAnimator.SetBool("Idle", false);
    }
}