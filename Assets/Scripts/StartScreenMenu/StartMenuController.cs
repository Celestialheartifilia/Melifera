using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("MainGameScene");
    }



    public void OpenOptions()
    {
        // "Additive" keeps the current scene active underneath
        //SceneManager.LoadScene("OptionMenuScene", LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName("OptionMenuScene");

        if (!scene.isLoaded)
            SceneManager.LoadScene("OptionMenuScene", LoadSceneMode.Additive);

    }



    public void CloseOptions()
    {
        //SceneManager.UnloadSceneAsync("OptionMenuScene");
        Scene scene = SceneManager.GetSceneByName("OptionMenuScene");

        if (scene.isLoaded)
            SceneManager.UnloadSceneAsync("OptionMenuScene");

        // IMPORTANT: restore UI based on current scene if not ui dissapears
        TopNavUI.instance.UpdateUI(SceneManager.GetActiveScene().name);


    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
        Application.Quit();
    }

   
}
