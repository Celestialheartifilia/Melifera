using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public void OnStartClick()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
        StartCoroutine(LoadSceneDelay("IntroCutScene"));
    }

    public void TempStartClick()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
        StartCoroutine(LoadSceneDelay("MainGameScene"));
    }

    public void OpenOptions()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);

        if (OptionsMenuController.Instance != null)
            OptionsMenuController.Instance.OpenOptions();
    }

    public void CloseOptions()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);

        if (OptionsMenuController.Instance != null)
            OptionsMenuController.Instance.CloseOptions();
    }

    public void OnExitClick()
    {
        SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    IEnumerator LoadSceneDelay(string sceneName)
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneName);
    }
}