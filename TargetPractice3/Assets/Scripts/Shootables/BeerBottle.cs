using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottle : MonoBehaviour
{
    [SerializeField] private List<Rigidbody> allParts = new List<Rigidbody>();
    [SerializeField] private List<Collider> allColliders;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private Material regularColor;
    [SerializeField] private Material highContrastColor;
    [SerializeField] private GameObject floatingText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Camera cam;
    
    private Vector3 regularColliderSize = new Vector3(1.1f, 4.2f, 1f);
    private Vector3 bigColliderSize = new Vector3(2f, 5f, 2f);
    private TargetManager manager;

    public void Shatter()
    {
        foreach (var part in allParts)
        {
            part.isKinematic = false;
        }
    }
    
    public void SetManager(TargetManager m)
    {
        manager = m;
    }
    
    public void BigColliders()
    {
        collider.size = bigColliderSize;
    }

    public void RegularColliders()
    {
        collider.size = regularColliderSize;
    }
    public void HighContrastOn()
    {
        foreach (var part in allParts)
        {
            part.gameObject.GetComponent<Renderer>().material = highContrastColor;
        }
    }
    
    public void HighContrastOff()
    {
        foreach (var part in allParts)
        {
            part.gameObject.GetComponent<Renderer>().material = regularColor;
        }
    }

    private new void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            ScoreManager.Instance.BottlesHit();
            Shatter();
            DisableColliders();
            ScoreManager.Instance.AddPoints(20);
            if (floatingText != null && canvas != null)
            {
                GameObject ft = Instantiate(floatingText, canvas.transform);
                ft.GetComponent<FloatingText>().SetText("+" + 20);
                Debug.Log(other.transform != null);
                Vector2 screenPos = cam.WorldToScreenPoint(other.transform.position);
                ft.GetComponent<RectTransform>().position = screenPos;
            }
        }
    }
    
    private void DisableColliders()
    {
        foreach(var c in allColliders)
        {
            c.enabled = false;
        }
    }
}
