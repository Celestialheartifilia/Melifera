using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;

public class MoodBarTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Timer UI")]
    public Slider timerSlider;
    public Image handleImage;// Drag the "Handle" object from the Slider here
    public float gameTime;

    [Header("Mood Handle Sprites (3)")]
    public Sprite[] moodSprites; // Drag your 3 different mood icons here
    // Array to hold your 3 different Mood UI Panels
    public GameObject[] moodPopups;

    private float timeRemaining;
    private bool stopTimer;
    private bool pauseTimer;          // ? NEW: freeze timer when validating
    private int moodIndex = 0; // Tracks which popup to show next

    [Header("Scoring System")]
    public GameObject[] honeyCombSprites;

    public int scoreValue;

    // TMP text to show score
    public TMP_Text scoreText;

    void Start()
    {
        scoreValue = 0;
        UpdateScoreText();
        UpdateHoneycombs();

        stopTimer = false;
        pauseTimer = false;          // ? NEW
        timeRemaining = gameTime;
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;

        foreach (GameObject popup in moodPopups)
        {
            if (popup != null) popup.SetActive(false);
        }

        TriggerNextMood();
    }

    void Update()
    {
        //// Subtract the time passed since the last frame
        if (stopTimer || pauseTimer) return;   // ? UPDATED pausetimer

        timeRemaining -= Time.deltaTime;
        float timeElapsed = gameTime - timeRemaining;
        // Trigger 2nd mood at 26.67s
        if (moodIndex == 1 && timeElapsed >= 26.67f)
            TriggerNextMood();
        // Trigger 3rd mood at 53.33s
        if (moodIndex == 2 && timeElapsed >= 53.33f)
            TriggerNextMood();

        if (timeRemaining <= 0)
        {
            stopTimer = true;
            timeRemaining = 0;
        }

        timerSlider.value = timeRemaining;
    }

    void TriggerNextMood()
    {
        if (moodIndex < moodSprites.Length)
        {
            // CHANGE THE HANDLE IMAGE
            if (handleImage != null && moodSprites[moodIndex] != null)
                handleImage.sprite = moodSprites[moodIndex];

            // POPUP LOGIC
            if (moodIndex < moodPopups.Length)
            {
                if (moodIndex > 0) moodPopups[moodIndex - 1].SetActive(false);
                moodPopups[moodIndex].SetActive(true);
            }

            moodIndex++;
        }
    }

    // Call this from your UI Buttons to close whatever  mood is open
    public void CloseCurrentMood()
    {
        foreach (GameObject popup in moodPopups)
            popup.SetActive(false);
        // Time.timeScale = 1;

        pauseTimer = false; // ?NEW: resume timer when closing popup
    }

    // =========================
    // VALIDATE BUTTON FUNCTION
    // =========================
    public void OnValidateButtonPressed()
    {
        pauseTimer = true;        // ? NEW: freeze bar when validating
        ValidateOrder(true);      // assume correct order
    }

    public void OnValidateButtonPressedWrong()
    {
        pauseTimer = true;        // ? NEW
        ValidateOrder(false);
    }

    // =========================
    // SCORE LOGIC
    // =========================
    public void ValidateOrder(bool isCorrect)
    {
        if (!isCorrect || stopTimer || timerSlider.normalizedValue <= 0f)
        {
            scoreValue = 0;
            UpdateScoreText();
            UpdateHoneycombs();
            return;
        }

        float bar = timerSlider.normalizedValue; // 0..1
        int multiplier;

        if (bar >= 0.7f)
            multiplier = 4;
        else if (bar >= 0.3f)
            multiplier = 2;
        else
            multiplier = 1;

        scoreValue = 10 * multiplier; // 40 / 20 / 10

        UpdateScoreText();
        UpdateHoneycombs();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + scoreValue;
    }

    void UpdateHoneycombs()
    {
        // Turn all off first can change the value for Final for now is 1 customer
        for (int i = 0; i < honeyCombSprites.Length; i++)
        {
            if (honeyCombSprites[i] != null)
                honeyCombSprites[i].SetActive(false);
        }

        if (scoreValue >= 10 && honeyCombSprites.Length > 0)
            honeyCombSprites[0].SetActive(true);

        if (scoreValue >= 20 && honeyCombSprites.Length > 1)
            honeyCombSprites[1].SetActive(true);

        if (scoreValue >= 40 && honeyCombSprites.Length > 2)
            honeyCombSprites[2].SetActive(true);
    }
}