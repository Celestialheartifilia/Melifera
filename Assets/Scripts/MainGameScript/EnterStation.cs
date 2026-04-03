using UnityEngine;
using UnityEngine.Events;

public class EnterStation : MonoBehaviour
{
    public UnityEvent onStationEnter;
    public Transform spawnPoint;

    void OnMouseDown()
    {
        Debug.Log("Station clicked: " + gameObject.name);
        DebugTextUI.Instance.Log("Station clicked");

        if (spawnPoint != null)
        {
            Debug.Log("Saving spawn point: " + spawnPoint.position);
            StationManager.playerSpawnPosition = spawnPoint.position;
            StationManager.hasSpawn = true;
        }
        else
        {
            Debug.LogWarning("Spawn point is null");
        }

        Debug.Log("About to invoke station event");
        DebugTextUI.Instance.Log("About to invoke station event");
        onStationEnter.Invoke();
        Debug.Log("Station event invoked");
        DebugTextUI.Instance.Log("Station event invoked");
    }
}