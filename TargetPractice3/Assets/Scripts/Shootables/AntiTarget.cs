using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiTarget : BaseShootable
{
    [SerializeField] private Transform pos0;
    [SerializeField] private Transform pos1;
    private AntiTargetManager manager;
    private int currentDestination = 0;
    private float speed = 2f;


    public void StartMovement()
    {
        StartCoroutine(MovementLoop());
    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetManager(AntiTargetManager m)
    {
        manager = m;
    }

    private IEnumerator MovementLoop()
    {
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
                GameObject ft = Instantiate(floatingText, canvas.transform);
                ft.GetComponent<FloatingText>().SetText("-" + 10);

                Vector2 screenPos = Camera.main.WorldToScreenPoint(other.transform.position);
                ft.GetComponent<RectTransform>().position = screenPos;
            }
        }
    }

    public override void AwardPoints()
    {
        ScoreManager.Instance.AddPoints(-10);
    }
}
