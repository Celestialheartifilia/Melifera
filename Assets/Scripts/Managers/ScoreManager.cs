using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TMP_Text scoreText;

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
}