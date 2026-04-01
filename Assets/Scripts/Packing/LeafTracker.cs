using UnityEngine;

public class LeafTracker : MonoBehaviour
{
    int totalLeaves;
    int removedLeaves;
    public GameObject PluckLeaves;

    PackingManager packingManager;

    void Start()
    {
        //PluckLeaves.SetActive(true);
        packingManager = FindObjectOfType<PackingManager>();

        totalLeaves = GetComponentsInChildren<LeafDispose>(true).Length;
        removedLeaves = 0;
    }

    public void NotifyLeafRemoved()
    {
        removedLeaves++;

        if (removedLeaves >= totalLeaves)
        {
            //PluckLeaves.SetActive(false);
            Debug.Log("All leaves plucked!");
            packingManager.OnLeavesPlucked();
        }
    }

    public void ResetLeaves()
    {
        removedLeaves = 0;

        LeafDispose[] leaves = GetComponentsInChildren<LeafDispose>(true);

        foreach (LeafDispose leaf in leaves)
        {
            leaf.gameObject.SetActive(true);
        }

        //PluckLeaves.SetActive(true);
    }
}
