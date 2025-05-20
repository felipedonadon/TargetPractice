using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : BaseShootable
{
    [SerializeField] private Transform pos0;
    [SerializeField] private Transform pos1;
    private int currentDestination = 0;
    private float speed = 2f;
    private TargetManager manager;


    public void StartMovement()
    {
        StartCoroutine(MovementLoop());
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetManager(TargetManager m)
    {
        manager = m;
    }

    private IEnumerator MovementLoop()
    {
        Debug.Log("started loop to " + currentDestination);
        Vector3 destination;
        if (currentDestination == 1)
        {
            destination = pos1.position;
        }
        else
        {
            destination = pos0.position;
        }

        var localStart = transform.position;
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector3.Lerp(localStart, destination, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        transform.position = destination;
        currentDestination = (currentDestination + 1) % 2;
        StartMovement();
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
                Debug.Log("floating text");
                string points;
                GameObject ft;
                if (inPenaltyMode)
                {
                    ft  = Instantiate(floatingTextBad, canvas.transform);
                    points = "-" + 15;
                }
                else
                {
                    ft  = Instantiate(floatingText, canvas.transform);
                    points = "+" + 15;
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
        int points = 15;
        if (inPenaltyMode)
        {
            points = -15;
        }
        ScoreManager.Instance.AddPoints(points);
        ScoreManager.Instance.TargetsHit();
    }
}
