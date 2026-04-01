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
        // When scene starts -> fade OUT (black -> clear)
        if (fadePanel != null)
            StartCoroutine(FadeOutOnStart());
    }

    IEnumerator FadeOutOnStart()
    {
        fadePanel.gameObject.SetActive(true);

        // start fully black
        SetAlpha(1f);

        yield return StartCoroutine(Fade(1, 0));

        // disable so it doesn't block UI
        fadePanel.gameObject.SetActive(false);
    }

    // =============================
    // SCENE LOAD FUNCTIONS
    // =============================
    public void LoadStartScene() => StartCoroutine(LoadScene("StartScene"));
    public void LoadMainGameScene() => StartCoroutine(LoadScene("MainGameScene"));
    public void LoadOrderTakingScene() => StartCoroutine(LoadScene("OrderTakingScene"));
    public void LoadHybridingFlowerScene() => StartCoroutine(LoadScene("HybridingFlowerScene"));
    public void LoadPackingScene() => StartCoroutine(LoadScene("PackingScene"));
    public void LoadOptionMenuScene() => StartCoroutine(LoadScene("OptionMenuScene"));

    IEnumerator LoadScene(string sceneName)
    {
        // enable panel so we can fade in
        fadePanel.gameObject.SetActive(true);

        yield return StartCoroutine(Fade(0, 1)); // fade IN to black

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