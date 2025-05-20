using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetManager : ShootableManager
{
    [SerializeField]
    private List<Target> targets;
    [SerializeField] 
    private List<StaticTarget> staticTargets;
    [SerializeField]
    private List<BeerBottle> beers;
    [SerializeField] 
    private float speed;

    [SerializeField]
    private Material regularColor;
    [SerializeField]
    private Material highContrastColor;
    [SerializeField]
    private Material badRegularColor;
    [SerializeField]
    private Material badHighContrastColor;
    [SerializeField]
    private Material transitionColor;
    
    public List<Target> Targets { get; private set; }

    private float baseSphereColliderSize;
    private float baseBoxColliderSize;
    
    

    private void Start()
    {
        ChangeSpeed();
        foreach (var target in targets)
        {
            target.SetManager(this);
            target.ChangeSpeed(speed);
            target.StartMovement();
        }
        foreach (var target in staticTargets)
        {
            target.SetManager(this);
        }
        foreach (var target in beers)
        {
            target.SetManager(this);
        }
        SetUpTargets();
        WaitBeforeChangingColor();
    }

    private void SetUpTargets()
    {
        List<BaseShootable> allTargets = new List<BaseShootable>();
        allTargets.AddRange(targets.Select(t => t));
        allTargets.AddRange(staticTargets.Select(t => t));

        int toFadeOut = allTargets.Count / 4;
        int t = 0;

        while (t <= toFadeOut)
        {
            int index = UnityEngine.Random.Range(0, allTargets.Count);
            var randomTarget = allTargets[index];
            
            randomTarget.DisableColliders();
            Color c = randomTarget.objRenderer.material.color;
            c.a = 0;
            randomTarget.objRenderer.material.color = c;
            randomTarget.fadedOut = true;
        }
    }

    private void WaitBeforeChangingColor()
    {
        StartCoroutine(WaitBeforeChangingColorLoop());
    }

    private IEnumerator WaitBeforeChangingColorLoop()
    {
        yield return new WaitForSeconds(5);
        StartChangingColour();
    }

    private void StartChangingColour()
    {
        List<BaseShootable> allTargets = new List<BaseShootable>();
        allTargets.AddRange(targets.Select(t => t));
        allTargets.AddRange(staticTargets.Select(t => t));

        var enabledTargets = allTargets
            .Where(t => t.fadedOut == false)
            .Where(t => t.inPenaltyMode == false)
            .ToList();
        var enabledTargetsWithPenalty = allTargets
            .Where(t => t.fadedOut == false)
            .Where(t => t.inPenaltyMode)
            .ToList();
        
        if (enabledTargets.Count > 0)
        {
            int temp = 0;
            while (temp < enabledTargets.Count/2)
            {
                int index = UnityEngine.Random.Range(0, enabledTargets.Count);
                if (GameSettings.HighContrastModeOn)
                {
                    enabledTargets[index].ChangeColor(badHighContrastColor, transitionColor);
                    enabledTargets.RemoveAt(index);
                }
                else
                {
                    enabledTargets[index].ChangeColor(badRegularColor, transitionColor);
                    enabledTargets.RemoveAt(index);
                }

                temp++;
            }
        }
        if (enabledTargetsWithPenalty.Count > 0)
        {
            int temp = 0;
            while (temp < enabledTargetsWithPenalty.Count/2)
            {
                int index = UnityEngine.Random.Range(0, enabledTargetsWithPenalty.Count);
                if (GameSettings.HighContrastModeOn)
                {
                    enabledTargetsWithPenalty[index].ChangeColor(highContrastColor, transitionColor);
                    enabledTargetsWithPenalty.RemoveAt(index);
                }
                else
                {
                    enabledTargetsWithPenalty[index].ChangeColor(regularColor, transitionColor);
                    enabledTargetsWithPenalty.RemoveAt(index);
                }

                temp++;
            }
        }
        WaitBeforeChangingColor();
    }
    
    public void UpdateSettings()
    {
        ChangeSpeed();
        if (GameSettings.HighContrastModeOn)
        {
            HighContrastOn();
        }
        else
        {
            HighContrastOff();
        }

        if (GameSettings.BiggerHitboxesOn)
        {
            BiggerHitboxes();
        }
        else
        {
            RegularHitboxes();
        }
    }

    public void EnableNewTarget()
    {
        List<BaseShootable> allTargets = new List<BaseShootable>();
        allTargets.AddRange(targets.Select(t => t));
        allTargets.AddRange(staticTargets.Select(t => t));

        var disabledTargets = allTargets
            .Where(t => t.fadedOut)
            .ToList();

        if (disabledTargets.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, disabledTargets.Count);
            var randomTarget = disabledTargets[index];
            if (GameSettings.HighContrastModeOn)
            {
                randomTarget.objRenderer.material = highContrastColor;
            }
            else
            {
                randomTarget.objRenderer.material = regularColor;
            }
            randomTarget.FadeIn();
        }
        else
        {
            Debug.Log("No disabled targets left to enable.");
        }
    }

    private void ChangeSpeed()
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
        
        foreach (var target in targets)
        {
            target.ChangeSpeed(speed);
        }
    }

    private void HighContrastOn()
    {
        foreach (var target in targets)
        {
            target.gameObject.GetComponent<Renderer>().material = highContrastColor;
        }
        foreach (var target in staticTargets)
        {
            target.gameObject.GetComponent<Renderer>().material = highContrastColor;
        }

        foreach (var beer in beers)
        {
            beer.HighContrastOn();
        }
    }

    private void HighContrastOff()
    {
        foreach (var target in targets)
        {
            target.gameObject.GetComponent<Renderer>().material = regularColor;
        }
        foreach (var target in staticTargets)
        {
            target.gameObject.GetComponent<Renderer>().material = regularColor;
        }
        foreach (var beer in beers)
        {
            beer.HighContrastOff();
        }
    }

    private void BiggerHitboxes()
    {
        foreach (var target in targets)
        {
            target.BigColliders();
        }
        foreach (var target in staticTargets)
        {
            Debug.Log("changing radius");
            target.BigColliders();
        }

        foreach (var beer in beers)
        {
            beer.BigColliders();
        }
    }

    private void RegularHitboxes()
    {
        foreach (var target in targets)
        {
            target.RegularColliders();
        }
        foreach (var target in staticTargets)
        {
            target.RegularColliders();
        }
        foreach (var beer in beers)
        {
            beer.RegularColliders();
        }
    }
}
