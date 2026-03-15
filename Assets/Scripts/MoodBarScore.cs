using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoodBarScore : MonoBehaviour
{
    public static MoodBarScore Instance;

    [Header("Mood Bar")]
    public Image fillImage; // foreground fill
    public float totalTime = 60f;

    float timeRemaining;
    bool timerRunning = true;

    [Header("Score")]
    public int baseScore = 10;
    public int scoreValue;

    public TMP_Text scoreText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ResetMoodBar();
        UpdateScoreText();
    }

    void Update()
    {
        if (!timerRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;
        }

        float percent = timeRemaining / totalTime;

        // animate the bar draining
        fillImage.fillAmount = percent;
    }

    public void ValidateOrder(bool isCorrect)
    {
        timerRunning = false;

        if (!isCorrect || fillImage.fillAmount <= 0f)
        {
            scoreValue = 0;
            UpdateScoreText();
            return;
        }

        float bar = fillImage.fillAmount;

        int multiplier;

        if (bar >= 0.667f)
            multiplier = 4;
        else if (bar >= 0.333f)
            multiplier = 2;
        else
            multiplier = 1;

        scoreValue = baseScore * multiplier;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = scoreValue.ToString();
    }

    public void ResetMoodBar()
    {
        timeRemaining = totalTime;
        timerRunning = true;

        if (fillImage != null)
            fillImage.fillAmount = 1f;
    }
}
