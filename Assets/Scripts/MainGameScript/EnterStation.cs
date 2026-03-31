using UnityEngine;
using UnityEngine.Events;

public class EnterStation : MonoBehaviour
{
    public UnityEvent onStationEnter;
    public Transform spawnPoint; //assign same as station target point

    void OnMouseDown()
    {
        Debug.Log("Station clicked: " + gameObject.name);
        //Save spawn position before changing scene
        if (spawnPoint != null)
        {
            StationManager.playerSpawnPosition = spawnPoint.position;
            StationManager.hasSpawn = true;
        }

        onStationEnter.Invoke();
    }
}