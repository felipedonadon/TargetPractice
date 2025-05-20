using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticAntiTarget : BaseShootable
{
    private AntiTargetManager manager;

    public void SetManager(AntiTargetManager m)
    {
        manager = m;
    }
    public override void AwardPoints()
    {
        ScoreManager.Instance.AddPoints(-10);
    }
    
    private new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AwardPoints();
            FadeOut();
            manager.EnableNewTarget();
            if (floatingText != null && canvas != null)
            {
                GameObject ft = Instantiate(floatingText, canvas.transform);
                ft.GetComponent<FloatingText>().SetText("-" + 10);

                Vector2 screenPos = Camera.main.WorldToScreenPoint(other.transform.position);
                ft.GetComponent<RectTransform>().position = screenPos;
            }
        }
    }
}
