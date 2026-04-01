using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;

    [Header("Final Score Screen")]
    public GameObject finalScorePanel;
    public TMP_Text finalScoreText;

    int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // keeps score across scenes
        }
        else
        {
            Destroy(gameObject); // prevent duplicate managers
            return;
        }

        UpdateScore();
    }

    public void AddPoints(int points)
    {
        score += points;

        Debug.Log("Score added: " + points + " | Total: " + score);

        UpdateScore();
    }

    void UpdateScore()
    {
        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TMP_Text>();
        }

        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void ShowFinalScore()
    {
        Debug.Log("Final Score: " + score);
        if (finalScorePanel != null)
            finalScorePanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "" + score;
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScore();
    }

    public void OnEndDayClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndingCutScene");
    }
}