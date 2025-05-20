using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTarget : BaseShootable
{
    private TargetManager _manager;

    public void SetManager(TargetManager m)
    {
        _manager = m;
    }
    
    private new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AwardPoints();
            FadeOut();
            _manager.EnableNewTarget();
            if (floatingText != null && canvas != null)
            {
                GameObject ft;
                string points;
                if (inPenaltyMode)
                {
                    ft = Instantiate(floatingTextBad, canvas.transform);
                    points = "-" + 10;
                }
                else
                {
                    ft  = Instantiate(floatingText, canvas.transform);
                    points = "+" + 10;
                }
                ft.GetComponent<FloatingText>().SetText(points);

                Vector2 screenPos = Camera.main.WorldToScreenPoint(other.transform.position);
                ft.GetComponent<RectTransform>().position = screenPos;
            }
            ScoreManager.Instance.TargetsHit();
        }
    }
    
    public override void AwardPoints()
    {
        int points = 10;
        if (inPenaltyMode)
        {
            points = -10;
        }
        ScoreManager.Instance.AddPoints(points);
        ScoreManager.Instance.TargetsHit();
    }
}
