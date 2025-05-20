using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private Scene level1;

    private void Awake()
    {
        void Awake()
        {
            // Singleton pattern to avoid duplicates
            if (Instance != null && Instance != this)
            {
                Debug.Log("Manager destroyed");
                Destroy(gameObject); // If a copy already exists, destroy this one
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // <- This keeps it alive between scenes
        }
    }
}
