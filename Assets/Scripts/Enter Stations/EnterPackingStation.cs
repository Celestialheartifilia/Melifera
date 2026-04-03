using UnityEngine;

public class EnterPackingStation : MonoBehaviour
{
    public Transform spawnPoint;
    public SceneLoader sceneLoader;
    public MainGameTutorial mainGameTutorial;

    void OnMouseDown()
    {
        Debug.Log("Packing station clicked: " + gameObject.name);

        if (spawnPoint != null)
        {
            StationManager.playerSpawnPosition = spawnPoint.position;
            StationManager.hasSpawn = true;
        }
        else
        {
            Debug.LogWarning("Packing station spawnPoint is null");
        }

        if (mainGameTutorial != null)
            mainGameTutorial.EndTutorialPacking();

        if (sceneLoader != null)
            sceneLoader.LoadPackingScene();
        else
            Debug.LogError("SceneLoader is null on Packing Station");
    }
}