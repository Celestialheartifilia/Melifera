using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
        StartCoroutine(Delay());
        SceneManager.LoadScene("IntroCutScene");
    }

    public void TempStartClick()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
        StartCoroutine(Delay());
        SceneManager.LoadScene("MainGameScene");
    }



    public void OpenOptions()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
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

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f); // small delay
    }


}
