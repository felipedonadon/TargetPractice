using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    
    [SerializeField] private Scene level1;
    public static bool AimAssistOn;
    public static bool AutoShootOn;
    public static bool BiggerHitboxesOn;
    public static bool HighContrastModeOn;
    public static bool PauseWhileSubtitlesOn;
    public static int TargetSpeed;
    public static float AudioVolume;
    public static int points;

    public static GameSettings Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AimAssistOn = false;
        AutoShootOn = false;
        BiggerHitboxesOn = false;
        HighContrastModeOn = false;
        PauseWhileSubtitlesOn = false;
        TargetSpeed = 1;
        AudioVolume = 1;
        points = 0;
    }
    
}