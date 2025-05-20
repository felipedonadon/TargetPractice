using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    
    [Header("Managers")] 
    [SerializeField] private TargetManager _targetManager;
    
    [Header("Look Parameters")] 
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upDownLookRange = 80f;
    [SerializeField] private float leftRightLookRange = 60f;
    [SerializeField] private LayerMask aimAssistLayer;
    [SerializeField] private LayerMask targetLayer;

    [Header("Shooting Parameters")] 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private float bulletVelocity;
    [SerializeField] private float reloadTime;
    private bool canShoot = true;

    [Header("Aim Assist Parameters")] 
    [SerializeField] private float aimAssistStrength = 100f;
    [SerializeField] private float maxAssistAngle = 10f;
    [SerializeField] private float aimAssistRange = 100f;
    [SerializeField] private float maxRotationPerSecond = 90f;
    [SerializeField] private float minAimAssistStrength = 0.1f;
    [SerializeField] private float assistSmoothTime = 0.1f;
    [SerializeField] private bool autoShoot;
    [SerializeField] private bool hasAimAssist;
    private bool activateAimAssist = false;

    [Header("References")] 
    //[SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private Transform cameraTransform;
    private float verticalRotation = 0;
    private float horizontalRotation = 0;
    private float currentAssistStrength = 0;
    private float assistVelocity = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleRotation();
        HandleShooting();
        if (GameSettings.AimAssistOn)
        {
            //HandleAimAssist();
            HandleAimAssistNew();
        }
    }
    
    private void HandleRotation()
    {
        float mouseX = _playerInputHandler.RotationInput.x * mouseSensitivity;
        float mouseY = _playerInputHandler.RotationInput.y * mouseSensitivity;
        
        horizontalRotation += mouseX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -55f, 55f); // Clamp horizontal Y rotation
        cameraHolder.localRotation = Quaternion.Euler(0f, horizontalRotation, 0f);
        
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleAimAssistNew()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, aimAssistRange, aimAssistLayer))
        {
            Vector3 targetDirection = (hit.collider.bounds.center - mainCamera.transform.position).normalized;
            float angleToTarget = Vector3.Angle(mainCamera.transform.forward, targetDirection);

            if (angleToTarget <= maxAssistAngle)
            {
                // Calculate directional dot between mouse movement and screen-space target
                Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                Vector3 screenTargetPos = mainCamera.WorldToScreenPoint(hit.collider.bounds.center);
                Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
                Vector2 toTarget = ((Vector2)screenTargetPos - screenCenter).normalized;

                float directionDot = Vector2.Dot(mouseDelta.normalized, toTarget);
                float targetStrength = Mathf.Lerp(minAimAssistStrength, aimAssistStrength, Mathf.Clamp01((directionDot + 1f) / 2f));

                // Smooth assist strength using SmoothDamp
                currentAssistStrength = Mathf.SmoothDamp(currentAssistStrength, targetStrength, ref assistVelocity, assistSmoothTime);

                // Apply smoothed aim assist rotation
                Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
                Quaternion horizontalRotation = Quaternion.Euler(0f, desiredRotation.eulerAngles.y, 0f);
                Quaternion verticalRotationQuat = Quaternion.Euler(desiredRotation.eulerAngles.x, 0f, 0f);

                cameraHolder.rotation = Quaternion.RotateTowards(
                    cameraHolder.rotation,
                    horizontalRotation,
                    maxRotationPerSecond * Time.deltaTime * currentAssistStrength
                );

                cameraTransform.localRotation = Quaternion.RotateTowards(
                    cameraTransform.localRotation,
                    verticalRotationQuat,
                    maxRotationPerSecond * Time.deltaTime * currentAssistStrength
                );

                verticalRotation = cameraTransform.localEulerAngles.x;
                if (verticalRotation > 180f) verticalRotation -= 360f;

                return;
            }
        }

        // No target or out of angle – fade assist strength back down
        currentAssistStrength = Mathf.SmoothDamp(currentAssistStrength, 0f, ref assistVelocity, assistSmoothTime);
    }

    private void HandleAimAssist()
    {
        Transform targetTransform = AssistedAim();
        if (targetTransform != null)
        {
            RotateAim(targetTransform);
        }
    }

    private void HandleShooting()
    {
        //shooting with left click
        if (_playerInputHandler.ShootTriggered && canShoot)
        {
            Shoot();
        }
        
        //auto shoot
        if (GameSettings.AutoShootOn && canShoot && DetectTarget())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        canShoot = false;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Vector3 direction = ShootingDirection().normalized;
        bullet.transform.forward = direction;
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletVelocity, ForceMode.Impulse);
        ScoreManager.Instance.ShotFired();
        StartCoroutine(Reload(reloadTime));
    }

    private Transform AssistedAim()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Single.PositiveInfinity, aimAssistLayer))
        {
            return hit.transform;
        }
        return null;
    }
    
    private void RotateAim(Transform targetTransform)
    {
        transform.LookAt(targetTransform);
    }

    private bool DetectTarget()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Single.PositiveInfinity, targetLayer) && (hit.transform.CompareTag("Target") || hit.transform.CompareTag("Beer")))
        {
            return true;
        }
        return false;
    }
    private Vector3 ShootingDirection()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }
        Vector3 direction = targetPoint - bulletSpawn.position;
        return direction;
    }

    private IEnumerator Reload(float time)
    {
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
}

