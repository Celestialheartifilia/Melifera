using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image fadePanel;
    public float fadeDuration = 0.2f;
    public float delayBeforeLoad = 0.1f;

    void Start()
    {
        if (fadePanel != null)
            StartCoroutine(FadeOutOnStart());
    }

    IEnumerator FadeOutOnStart()
    {
        fadePanel.gameObject.SetActive(true);
        SetAlpha(1f);

        yield return StartCoroutine(Fade(1, 0));

        fadePanel.gameObject.SetActive(false);
    }

    public void LoadStartScene() => LoadSceneWithSFX("StartScene");
    public void LoadMainGameScene() => LoadSceneWithSFX("MainGameScene");
    public void LoadOrderTakingScene() => LoadSceneWithSFX("OrderTakingScene");
    public void LoadHybridingFlowerScene() => LoadSceneWithSFX("HybridingFlowerScene");
    public void LoadPackingScene() => LoadSceneWithSFX("PackingScene");
    public void LoadOptionMenuScene() => LoadSceneWithSFX("OptionMenuScene");

    void LoadSceneWithSFX(string sceneName)
    {
        if (SoundEffectPlayer.Instance != null)
            SoundEffectPlayer.Instance.PlaySound(SoundEffectPlayer.Instance.buttonClickSFX);

        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        fadePanel.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(0, 1));
        yield return new WaitForSeconds(delayBeforeLoad);

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0;
        Color color = fadePanel.color;

        while (time < fadeDuration)
        {
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            fadePanel.color = color;

            time += Time.deltaTime;
            yield return null;
        }

        color.a = endAlpha;
        fadePanel.color = color;
    }

    void SetAlpha(float alpha)
    {
        Color c = fadePanel.color;
        c.a = alpha;
        fadePanel.color = c;
    }
}