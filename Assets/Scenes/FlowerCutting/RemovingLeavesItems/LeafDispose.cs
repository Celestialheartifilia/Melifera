using UnityEngine;

public class LeafDispose : MonoBehaviour
{
    [Header("Reference")]
    public Collider2D binCollider;

    LeafTracker leafTracker;

    void Start()
    {
        leafTracker = GetComponentInParent<LeafTracker>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != binCollider) return;

        leafTracker?.NotifyLeafRemoved();
        
        gameObject.SetActive(false); // disable only
    }

    public void ResetLeaf()
    {
        gameObject.SetActive(true);
    }
}
