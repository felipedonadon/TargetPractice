using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetBaloon : Target
{
    [SerializeField] private float scaleSpeed;
    [SerializeField] private Vector3 startScale;
    [SerializeField] private Vector3 endScale;
    private bool isScaling;
    private float t = 0;

    private void Start()
    {
        //ReduceSize();
        isScaling = true;
        transform.localScale = startScale;
    }

    public void ReduceSize()
    {
        StartCoroutine(ReduceSizeLoop());
    }

    private IEnumerator ReduceSizeLoop()
    {
        float t = 0;
        while (t < 1)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
            t += Time.deltaTime / scaleSpeed;
            
            if (t % 0.1f < 0.02f)
            {
                DisableColliders();
                EnableColliders();
            }
            yield return null;
        }
        
        DisableColliders();
        EnableColliders();
        transform.localScale = endScale;
    }
    
    private void FixedUpdate()
    {
        if (!isScaling) return;

        t += Time.fixedDeltaTime / scaleSpeed;

        if (t >= 1f)
        {
            t = 1f;
            isScaling = false;
        }

        transform.localScale = Vector3.Lerp(startScale, endScale, t);

        // Optionally: Force a collider refresh to ensure detection
        DisableColliders();
        EnableColliders();
    }
}
