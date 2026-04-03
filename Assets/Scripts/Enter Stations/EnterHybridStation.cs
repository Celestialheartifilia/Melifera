using UnityEngine;

public class EnterHybridStation : MonoBehaviour
{
    public Transform spawnPoint;
    public SceneLoader sceneLoader;
    public MainGameTutorial mainGameTutorial;

    void OnMouseDown()
    {
        Debug.Log("Hybrid station clicked: " + gameObject.name);

        if (spawnPoint != null)
        {
            StationManager.playerSpawnPosition = spawnPoint.position;
            StationManager.hasSpawn = true;
        }
        else
        {
            Debug.LogWarning("Hybrid station spawnPoint is null");
        }

        if (mainGameTutorial != null)
            mainGameTutorial.EndTutorialHybrid();

        if (sceneLoader != null)
            sceneLoader.LoadHybridingFlowerScene();
        else
            Debug.LogError("SceneLoader is null on Hybrid Station");
    }
}