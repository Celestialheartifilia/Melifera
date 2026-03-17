using UnityEngine;
using System.Collections;

public class LeafDispose : MonoBehaviour
{
    [Header("Reference")]
    public Collider2D binCollider;

    LeafTracker leafTracker;
    bool disposed = false;

    void Start()
    {
        leafTracker = GetComponentInParent<LeafTracker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (disposed) return;
        if (other != binCollider) return;

        disposed = true;

        leafTracker?.NotifyLeafRemoved();

        StartCoroutine(DisableLeaf());
    }

    IEnumerator DisableLeaf()
    {
        yield return new WaitForSeconds(0.3f); // adjust if needed
        gameObject.SetActive(false);
    }

    public void ResetLeaf()
    {
        disposed = false;
        gameObject.SetActive(true);
    }
}