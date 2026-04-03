using UnityEngine;

public class EnterOrderStation : MonoBehaviour
{
    public Transform spawnPoint;
    public SceneLoader sceneLoader;
    public MainGameTutorial mainGameTutorial;

    void OnMouseDown()
    {
        Debug.Log("Order station clicked: " + gameObject.name);

        if (spawnPoint != null)
        {
            StationManager.playerSpawnPosition = spawnPoint.position;
            StationManager.hasSpawn = true;
        }
        else
        {
            Debug.LogWarning("Order station spawnPoint is null");
        }

        if (mainGameTutorial != null)
            mainGameTutorial.EndTutorial();

        if (sceneLoader != null)
            sceneLoader.LoadOrderTakingScene();
        else
            Debug.LogError("SceneLoader is null on Order Station");
    }
}