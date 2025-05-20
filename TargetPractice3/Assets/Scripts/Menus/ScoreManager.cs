using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int PointsCount => points;

    public int ShotsFiredCount => shotsFired;

    public int TargetsHitCount => targetsHit;

    public int BottlesHitCount => bottlesHit;

    public TMP_Text ScoreText => scoreText;

    public static ScoreManager Instance { get; private set; }
    private int points = 0;
    private int shotsFired = 0;
    private int targetsHit = 0;
    private int bottlesHit = 0;
    public TMP_Text scoreText;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void AddPoints(int amount)
    {
        GameSettings.points += amount;
        points += amount;
        UpdateScoreUI();
    }

    public void ShotFired()
    {
        shotsFired += 1;
    }

    public void TargetsHit()
    {
        targetsHit += 1;
    }

    public void BottlesHit()
    {
        bottlesHit += 1;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + GameSettings.points;
    }

    public int GetScore()
    {
        return GameSettings.points;
    }
}
