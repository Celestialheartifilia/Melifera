using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("References")]
    public PollinationManager pollinationManager;

    [Header("Direction Animation")]
    public GameObject frontObj;
    public GameObject backObj;
    public GameObject leftObj;
    public GameObject rightObj;

    Rigidbody2D rb;

    Vector3 targetPosition;
    bool hasTarget = false;

    Vector3 startPosition;

    NormalFlower currentFlower;
    Pot currentPot;

    public bool canMoveToFlowers = true;
    public bool canMoveToPot = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        startPosition = transform.position;

        ShowOnly(frontObj);
    }

    void Update()
    {
        if (hasTarget)
        {
            MoveToTarget();
        }
    }

    void MoveToTarget()
    {
        Vector2 dir = (targetPosition - transform.position);

        if (dir.magnitude < 0.05f)
        {
            rb.linearVelocity = Vector2.zero;
            hasTarget = false;

            OnReachedTarget();
            return;
        }

        dir.Normalize();
        rb.linearVelocity = dir * moveSpeed;

        // Direction animation
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            ShowOnly(dir.x > 0 ? rightObj : leftObj);
        else
            ShowOnly(dir.y > 0 ? backObj : frontObj);
    }

    void OnReachedTarget()
    {
        //FLOWER LOGIC
        if (currentFlower != null && !currentFlower.isPollinated)
        {
            //SHOW INDICATOR ONLY WHEN REACHED
            currentFlower.ShowPollinateIndicator(true);

            // small delay feels nicer (optional)
            StartCoroutine(DoPollination(currentFlower));
            return;
        }

        //POT LOGIC
        if (currentPot != null && pollinationManager.PollinationCount == 2)
        {
            bool planted = pollinationManager.TryPlantInto(currentPot);

            if (planted)
            {
                ReturnToStart();
            }

            currentPot = null;
        }
    }

    IEnumerator DoPollination(NormalFlower flower)
    {
        yield return new WaitForSeconds(0.3f); // optional polish

        pollinationManager.TryAddPollinatedFlower(flower);

        flower.ShowPollinateIndicator(false);

        currentFlower = null;
    }

    // ===============================
    // Movement Commands
    // ===============================

    public void MoveToFlower(NormalFlower flower)
    {
        if (!canMoveToFlowers)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("You can't pollinate now.");
            return;
        }

        if (pollinationManager.PollinationCount >= 2)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("You already selected 2 flowers. Plant or clear first.");
            return;
        }

        if (flower.isPollinated)
            return;

        if (currentFlower != null)
            currentFlower.ShowPollinateIndicator(false);

        currentFlower = flower;
        currentPot = null;

        targetPosition = flower.transform.position;
        hasTarget = true;
    }

    public void MoveToPot(Pot pot)
    {

        if (pollinationManager.PollinationCount < 2)
            return;

        if (currentFlower != null)
            currentFlower.ShowPollinateIndicator(false);

        currentPot = pot;
        currentFlower = null;

        targetPosition = pot.transform.position;
        hasTarget = true;
    }

    public void ReturnToStart()
    {
        if (currentFlower != null)
            currentFlower.ShowPollinateIndicator(false);

        currentFlower = null;
        currentPot = null;

        targetPosition = startPosition;
        hasTarget = true;
    }

    // ===============================
    // Animation
    // ===============================

    void ShowOnly(GameObject obj)
    {
        if (frontObj) frontObj.SetActive(obj == frontObj);
        if (backObj) backObj.SetActive(obj == backObj);
        if (leftObj) leftObj.SetActive(obj == leftObj);
        if (rightObj) rightObj.SetActive(obj == rightObj);
    }
}