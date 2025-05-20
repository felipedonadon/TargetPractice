using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AntiTargetManager : MonoBehaviour
{
    [SerializeField]
    private List<AntiTarget> antiTargets;
    [SerializeField] 
    private List<StaticAntiTarget> staticAntiTargets;
    [SerializeField] 
    private float speed;

    [SerializeField]
    private Material regularColor;
    [SerializeField]
    private Material highContrastColor;
    
    public List<Target> Targets { get; private set; }
    
    

    private void Start()
    {
        ChangeSpeed();
        foreach (var target in antiTargets)
        {
            target.ChangeSpeed(speed);
            target.StartMovement();
            target.SetManager(this);
        }
        foreach (var target in staticAntiTargets)
        {
            target.SetManager(this);
        }
    }

    public void UpdateSettings()
    {
        Debug.Log("updating");
        ChangeSpeed();
        if (GameSettings.HighContrastModeOn)
        {
            HighContrastOn();
        }
        else
        {
            HighContrastOff();
        }
    }

    public void ChangeSpeed()
    {
        if (GameSettings.TargetSpeed == 0)
        {
            speed = 0.1f;
        }

        if (GameSettings.TargetSpeed == 1)
        {
            speed = 0.2f;
        }

        if (GameSettings.TargetSpeed == 2)
        {
            speed = 0.4f;
        }
        
        foreach (var target in antiTargets)
        {
            target.ChangeSpeed(speed);
            target.StartMovement();
        }
    }
    
    public void EnableNewTarget()
    {
        List<BaseShootable> allTargets = new List<BaseShootable>();
        allTargets.AddRange(antiTargets.Select(t => t));
        allTargets.AddRange(staticAntiTargets.Select(t => t));

        var disabledTargets = allTargets
            .Where(t => t.fadedOut)
            .ToList();

        if (disabledTargets.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, disabledTargets.Count);
            var randomTarget = disabledTargets[index];

            randomTarget.FadeIn();
        }
        else
        {
            Debug.Log("No disabled targets left to enable.");
        }
    }

    public void HighContrastOn()
    {
        foreach (var target in antiTargets)
        {
            target.gameObject.GetComponent<Renderer>().material = highContrastColor;
        }
        foreach (var target in staticAntiTargets)
        {
            target.gameObject.GetComponent<Renderer>().material = highContrastColor;
        }
    }

    public void HighContrastOff()
    {
        foreach (var target in antiTargets)
        {
            target.gameObject.GetComponent<Renderer>().material = regularColor;
        }
        foreach (var target in staticAntiTargets)
        {
            target.gameObject.GetComponent<Renderer>().material = regularColor;
        }
    }
}
