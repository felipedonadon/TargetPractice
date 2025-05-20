using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private int shotsTotal;
    private int targetsHit;
    private int staticTargetsHit;
    private int movingTargetsHit;
    private int bottlesHit;
    private int points;

    private void Start()
    {
        shotsTotal = 0;
        targetsHit = 0;
        staticTargetsHit = 0;
        movingTargetsHit = 0;
        bottlesHit = 0;
        points = 0;
    }

    public void AddPoints(int pointsGained)
    {
        points += pointsGained;
    }

    private void TargetHitCount()
    {
        targetsHit += 1;
    }
    
    public void MovingTargetsHitCount()
    {
        targetsHit += 1;
        movingTargetsHit += 1;
    }
    
    public void StaticTargetsHitCount()
    {
        targetsHit += 1;
        staticTargetsHit += 1;
    }
    
    public void ShotsFiredCount()
    {
        shotsTotal += 1;
    }
    
    public void BottlesHitCount()
    {
        bottlesHit += 1;
    }
}
