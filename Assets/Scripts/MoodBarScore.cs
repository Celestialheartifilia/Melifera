using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoodBarScore : MonoBehaviour
{
    public static MoodBarScore Instance;

    [Header("UI")]
    public GameObject moodBarObject;
    public Image fillImage;

    [Header("Timer")]
    public float totalTime = 60f;

    float timeRemaining;
    bool timerRunning;

    [Header("Score")]
    public int baseScore = 10;
    [SerializeField] int scoreValue;
    public TMP_Text scoreText;

    void Awake()
    {
        Instance = this;
        if (moodBarObject == null)
            moodBarObject = gameObject;

        moodBarObject.SetActive(false);

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
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

        if (fillImage != null)
            fillImage.fillAmount = percent;
    
}

    // Called when player takes the order
    public void StartMoodBar()
    {
        Debug.Log("Mood bar started");

        // reset timer
        timeRemaining = totalTime;

        // reset fill
        if (fillImage != null)
            fillImage.fillAmount = 1f;

        // show bar
        if (moodBarObject != null)
            moodBarObject.SetActive(true);

        // start timer
        timerRunning = true;
    }

    // Stops timer
    public void StopMoodBar()
    {
        timerRunning = false;

        Debug.Log("Mood bar stopped");
    }

    // Called by PackingManager
    public void ValidateOrder(bool isCorrect)
    {
        Debug.Log("ValidateOrder called");

        // stop timer immediately
        StopMoodBar();

        float bar = timeRemaining / totalTime;

        if (!isCorrect)
        {
            Debug.Log("Wrong order");
        }
        else
        {
            int multiplier;

            if (bar >= 0.667f)
                multiplier = 4;
            else if (bar >= 0.333f)
                multiplier = 2;
            else
                multiplier = 1;

            int points = baseScore * multiplier;

            scoreValue += points;

            Debug.Log("Points added: " + points);
        }

        UpdateScoreText();

        // hide bar after scoring
        if (moodBarObject != null)
            moodBarObject.SetActive(false);
    }

    public void UpdateScoreText()
    {
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        Debug.Log(scoreText);
        if (scoreText != null)
            scoreText.text = scoreValue.ToString();
    }
}