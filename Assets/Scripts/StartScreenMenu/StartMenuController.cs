using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene("IntroCutScene");
    }

    public void TempStartClick()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    public void OpenOptions()
    {
        // "Additive" keeps the current scene active underneath
        SceneManager.LoadScene("OptionMenuScene", LoadSceneMode.Additive);
    }

    public void CloseOptions()
    {
        SceneManager.UnloadSceneAsync("OptionMenuScene");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; 
#endif
        Application.Quit();
    }

   
}
