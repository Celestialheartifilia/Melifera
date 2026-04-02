using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsMenuController : MonoBehaviour
{
    public static OptionsMenuController Instance;

    private string previousSceneName;
    private GameObject inputBlocker;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetInputBlocker(GameObject blocker)
    {
        inputBlocker = blocker;
    }

    public void OpenOptions()
    {
        Scene scene = SceneManager.GetSceneByName("OptionMenuScene");

        if (!scene.isLoaded)
        {
            previousSceneName = SceneManager.GetActiveScene().name;

            PlayerMovementScript player = FindObjectOfType<PlayerMovementScript>();
            if (player != null)
                player.canMove = false;

            if (inputBlocker != null)
                inputBlocker.SetActive(true);

            SceneManager.LoadScene("OptionMenuScene", LoadSceneMode.Additive);
        }
    }

    public void CloseOptions()
    {
        StartCoroutine(CloseOptionsRoutine());
    }

    IEnumerator CloseOptionsRoutine()
    {
        Scene scene = SceneManager.GetSceneByName("OptionMenuScene");

        if (scene.isLoaded)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync("OptionMenuScene");

            while (!unloadOp.isDone)
                yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        yield return null;

        PlayerMovementScript player = FindObjectOfType<PlayerMovementScript>();
        if (player != null)
            player.canMove = true;

        if (TopNavUI.instance != null)
            TopNavUI.instance.UpdateUI(previousSceneName);

        yield return null;

        if (inputBlocker != null)
            inputBlocker.SetActive(false);
    }
}