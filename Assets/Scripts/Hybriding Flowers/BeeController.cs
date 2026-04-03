using System.Collections;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float stopDistance = 0.05f;
    public float faceThreshold = 0.1f;

    [Header("References")]
    public PollinationManager pollinationManager;

    [Header("Direction Animation")]
    public GameObject leftObj;
    public GameObject rightObj;

    [Header("Animator")]
    public Animator beeAnimator;

    Rigidbody2D rb;

    Vector3 targetPosition;
    bool hasTarget = false;

    Vector3 startPosition;
    Vector3 lastPosition;

    NormalFlower currentFlower;
    Pot currentPot;

    public bool canMoveToFlowers = true;
    public bool canMoveToPot = true;

    string currentAnim = "";

    public ParticleSystem pollinateParticle;

    private bool isPollinating = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }

        startPosition = transform.position;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (hasTarget)
        {
            MoveToTarget();
        }

        lastPosition = transform.position;
    }

    void MoveToTarget()
    {
        Vector3 currentPosition = transform.position;
        float distance = Vector3.Distance(currentPosition, targetPosition);

        if (distance <= stopDistance)
        {
            transform.position = targetPosition;
            hasTarget = false;

            OnReachedTarget();
            return;
        }

        Vector3 newPosition = Vector3.MoveTowards(
            currentPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        Vector3 moveDelta = newPosition - currentPosition;
        transform.position = newPosition;

        // only switch facing if movement in x is strong enough
        if (moveDelta.x > faceThreshold * Time.deltaTime)
        {
            PlayAnim("TinyBeeFlyRight");
        }
        else if (moveDelta.x < -faceThreshold * Time.deltaTime)
        {
            PlayAnim("TinyBeeFly");
        }
    }

    void PlayAnim(string animName)
    {
        if (beeAnimator == null) return;
        if (currentAnim == animName) return;

        beeAnimator.Play(animName);
        currentAnim = animName;
    }

    void OnReachedTarget()
    {
        if (currentFlower != null && !currentFlower.isPollinated)
        {
            currentFlower.ShowPollinateIndicator(true);
            ShowPollinateEffect();
            StartCoroutine(DoPollination(currentFlower));
            return;
        }

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
        flower.ShowPollinateIndicator(true);

        yield return new WaitForSeconds(0.5f);

        pollinationManager.TryAddPollinatedFlower(flower);
        flower.ShowPollinateIndicator(false);

        currentFlower = null;
        isPollinating = false;
    }

    public void MoveToFlower(NormalFlower flower)
    {
        if (isPollinating || hasTarget)
            return;

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
        isPollinating = true;
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

    public void ShowPollinateEffect()
    {
        if (pollinateParticle == null) return;

        pollinateParticle.Clear();
        pollinateParticle.Play();
    }

    public void StopPollinateEffect()
    {
        if (pollinateParticle == null) return;

        pollinateParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}