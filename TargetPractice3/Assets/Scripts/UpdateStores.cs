using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateStores : MonoBehaviour
{
    [SerializeField] private TargetManager _targetManager;
    [SerializeField] private AntiTargetManager _antiTargetManager;

    public void UpdateShootableStores()
    {
        _targetManager.UpdateSettings();
        _antiTargetManager.UpdateSettings();
    }
}

