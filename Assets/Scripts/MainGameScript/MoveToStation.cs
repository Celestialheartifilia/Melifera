using UnityEngine;

public class MoveToStation : MonoBehaviour
{
    public Transform targetPoint; // where player should go

    void OnMouseDown()
    {
        PlayerMovementScript player = FindObjectOfType<PlayerMovementScript>();
        if (player != null && targetPoint != null)
        {
            player.SetTarget(targetPoint.position);
        }
    }
}
