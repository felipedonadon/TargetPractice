using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")] [SerializeField]
    private InputActionAsset playerControls;

    [Header("Action Map Name Reference")] [SerializeField]
    private string actionMap = "ShootingState";

    [Header("Action Name references")] 
    [SerializeField]
    private string rotation = "Rotation";
    [SerializeField]
    private string shoot = "Shoot";
    [SerializeField] 
    private string pause = "Pause";

    private InputAction rotationAction;
    private InputAction shootAction;
    private InputAction pauseAction;
    
    public Vector2 RotationInput { get; private set; }
    
    public bool ShootTriggered { get; private set; }
    
    public bool PausePressed { get; private set; }

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMap);

        rotationAction = mapReference.FindAction(rotation);
        shootAction = mapReference.FindAction(shoot);
        pauseAction = mapReference.FindAction(pause);
        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        shootAction.performed += inputInfo => ShootTriggered = true;
        shootAction.canceled += inputInfo => ShootTriggered = false;

        pauseAction.performed += inputInfo => PausePressed = true;
        pauseAction.canceled += inputInfo => PausePressed = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMap).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMap).Disable();
    }
}
