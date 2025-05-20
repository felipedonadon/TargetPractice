using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private FirstPersonController fpc;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text pointsTarget;
    [SerializeField] private TMP_Text pointsBottle;
    [SerializeField] private TMP_Text pointsShooting;
    [SerializeField] private GameObject endInfoHolder;
    public float gameDuration = 60f; // 60 seconds
    private float elapsedTime = 0f;
    private bool gameEnded = false;


    void Update()
    {
        if (gameEnded) return;

        elapsedTime += Time.deltaTime;
        
        float timeLeft = Mathf.Max(0f, gameDuration - elapsedTime);
        UpdateCountdownUI(timeLeft);
        
        if (elapsedTime >= gameDuration)
        {
            EndGame();
        }
    }

    private void UpdateCountdownUI(float timeLeft)
    {
        int seconds = Mathf.CeilToInt(timeLeft);
        countdownText.text = seconds.ToString();
    }
    
    private void EndGame()
    {
        gameEnded = true;
        fpc.enabled = false;

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        endInfoHolder.SetActive(true);
        DisplayEndInfo();
    }

    private void DisplayEndInfo()
    {
        countdownText.enabled = false;
        pointsTarget.text = ScoreManager.Instance.TargetsHitCount.ToString();
        pointsShooting.text = ScoreManager.Instance.ShotsFiredCount.ToString();
        pointsBottle.text = ScoreManager.Instance.BottlesHitCount.ToString();
    }
}