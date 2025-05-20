using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseShootable : MonoBehaviour
{
    [SerializeField] public Renderer objRenderer;
    [SerializeField] private List<Collider> colliders;
    [SerializeField] private SphereCollider myCollider;
    [SerializeField] private float fadeSpeed = 1;
    [SerializeField] protected GameObject floatingText;
    [SerializeField] protected GameObject floatingTextBad;
    [SerializeField] protected Canvas canvas;
    public bool inPenaltyMode = false;
    private float regularColliderRadius = 0.5f;
    private float bigColliderRadius = 1;

    public bool fadedOut;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
    }

    protected void FadeOut()
    {
        StartCoroutine(FadeOutLoop());
    }

    public void FadeIn()
    {
        StartCoroutine(FadeInLoop());
    }

    public void DisableColliders()
    {
        foreach(var c in colliders)
        {
            c.enabled = false;
        }
    }

    protected void EnableColliders()
    {
        foreach (var c in colliders)
        {
            c.enabled = true;
        }
    }
    
    public void ChangeColor(Material newColor, Material transitionMaterial)
    {
        StartCoroutine(ChangeColorLoop(newColor, transitionMaterial));
    }

    private IEnumerator ChangeColorLoop(Material newColor, Material transitionMaterial)
    {
        float t = 0;
        while (t < 3)
        {
            Material currentMaterial = objRenderer.material;
            objRenderer.material = transitionMaterial;
            yield return new WaitForSeconds(0.5f);
            objRenderer.material = currentMaterial;
            t++;
            yield return new WaitForSeconds(0.5f);
        }
        inPenaltyMode = !inPenaltyMode;
        if (inPenaltyMode)
        {
            colliders[1].enabled = false;
        }
        else
        {
            colliders[1].enabled = true;
        }
        objRenderer.material = newColor;
    }

    private IEnumerator FadeOutLoop()
    {
        DisableColliders();
        float t = 0;
        while (t < 1)
        {
            Color currentColor = objRenderer.material.color;
            float tempAlpha = Mathf.Lerp(1, 0, t);
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, tempAlpha);
            objRenderer.material.color = newColor;
            t += Time.deltaTime / fadeSpeed;
            yield return null;
        }

        fadedOut = true;
    }

    private IEnumerator FadeInLoop()
    {
        float t = 0;
        while (t < 1)
        {
            Color currentColor = objRenderer.material.color;
            float tempAlpha = Mathf.Lerp(0, 1, t);
            Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, tempAlpha);
            objRenderer.material.color = newColor;
            t += Time.deltaTime / fadeSpeed;
            yield return null;
        }
        EnableColliders();
        fadedOut = false;
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AwardPoints();
            FadeOut();
            if (floatingText != null && canvas != null)
            {
                GameObject ft = Instantiate(floatingText, canvas.transform);
                ft.GetComponent<FloatingText>().SetText("+" + (-10));

                Vector2 screenPos = Camera.main.WorldToScreenPoint(other.transform.position);
                ft.GetComponent<RectTransform>().position = screenPos;
            }
        }
    }

    public void BigColliders()
    {
        myCollider.radius = bigColliderRadius;
    }

    public void RegularColliders()
    {
        myCollider.radius = regularColliderRadius;
    }

    public abstract void AwardPoints();
}
