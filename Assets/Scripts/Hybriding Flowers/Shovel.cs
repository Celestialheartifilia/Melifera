using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shovel : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;

    Vector2 offset;
    Vector2 startPos;

    bool dragging;

    Pot currentPot;
    Collider2D soilCollider;

    bool hasFertilised = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        startPos = rb.position;
    }

    public void ActivateShovel(Pot pot, Collider2D soil)
    {
        currentPot = pot;
        soilCollider = soil;
        dragging = true;
        hasFertilised = false;
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
        StartCoroutine(ResetShovel());
    }

    void FixedUpdate()
    {
        if (!dragging) return;

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        rb.MovePosition(mouseWorld + offset);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (soilCollider == null) return;
        if (other != soilCollider) return;
        if (hasFertilised) return;

        if (currentPot != null && currentPot.IsReadyToFertilise())
        {
            hasFertilised = true;
            currentPot.Fertilise();
            StartCoroutine(ResetShovel());
        }
    }

    IEnumerator ResetShovel()
    {
        yield return new WaitForSeconds(0.2f);

        rb.MovePosition(startPos);
        dragging = false;

        gameObject.SetActive(false);

        // Also reset fertiliser visuals
        FertilizerManager fert = FindObjectOfType<FertilizerManager>();
        if (fert != null)
            fert.ResetFertiliserState();
    }
}
