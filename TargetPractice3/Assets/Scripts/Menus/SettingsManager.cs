using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Toggles")]
    public Toggle aimAssistToggle;
    public Toggle autoShootToggle;
    public Toggle bigHitboxesToggle;
    public Toggle highContrastToggle;
    public Toggle pauseWhileSubtitlesToggle;

    [Header("Dropdown")]
    public TMP_Dropdown targetSpeedDropdown;

    [Header("Sliders")]
    public Slider audioSlider;
    public Slider brightnessSlider;

    void Start()
    {
        // Load saved values and apply to UI
        aimAssistToggle.isOn     = GameSettings.AimAssistOn;
        autoShootToggle.isOn     = GameSettings.AutoShootOn;
        bigHitboxesToggle.isOn   = GameSettings.BiggerHitboxesOn;
        highContrastToggle.isOn  = GameSettings.HighContrastModeOn;
        pauseWhileSubtitlesToggle.isOn = GameSettings.PauseWhileSubtitlesOn;

        targetSpeedDropdown.value = GameSettings.TargetSpeed;
        audioSlider.value         = GameSettings.AudioVolume;
    }

    public void OnLockOnChanged(bool value)
    {
        GameSettings.AimAssistOn = aimAssistToggle.isOn;
        //Debug.Log("Aim assist is on " + GameSettings.AimAssistOn);
    }
    
    public void OnAutoShootChanged(bool value)
    {
        GameSettings.AutoShootOn = autoShootToggle.isOn;
        //Debug.Log("AutoShoot changed to " + GameSettings.AutoShootOn);
    }
    
    public void OnBigHitboxesChanged(bool value)
    {
        GameSettings.BiggerHitboxesOn = bigHitboxesToggle.isOn;
        //Debug.Log("BigHitboxes changed to " + GameSettings.BiggerHitboxesOn);
    }
    
    public void OnHighContrastChanged(bool value)
    {
        GameSettings.HighContrastModeOn = highContrastToggle.isOn;
        //Debug.Log("High contrast changed to " + GameSettings.HighContrastModeOn);
    }

    public void OnPauseWhileSubtitlesChanged(bool value)
    {
        GameSettings.PauseWhileSubtitlesOn = pauseWhileSubtitlesToggle.isOn;
    }

    public void OnTargetSpeedChanged(int value)
    {
        GameSettings.TargetSpeed = targetSpeedDropdown.value;
        //Debug.Log("Target's speed changed to " + GameSettings.TargetSpeed);
    }

    public void OnAudioChanged(float value)
    {
        GameSettings.AudioVolume = audioSlider.value;
        //Debug.Log("Audio changed to " + GameSettings.AudioVolume);
    }
}