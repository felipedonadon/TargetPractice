using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletInstantiation;
    [SerializeField]
    private float bulletLifetime = 30f;
    [SerializeField] 
    private float bulletVelocity = 3f;
    
    void Update()
    {
        
    }
}
