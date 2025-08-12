using System;
using UnityEngine;

public class PlayerEventManager : MonoBehaviour
{
    public static PlayerEventManager _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public event Action<DamageSource, DamageType> OnPlayerDamaged;      
    public event Action<bool> OnPlayerTorchToggled;                      
    public event Action<int> OnPlayerQuickSlotSwitched;                  
    public event Action OnRetryEncounter;                                
    public event Action OnRequestRespawn;                                
    public event Action<ZoneType> OnEnteredZone;                         
    public event Action<float> OnMoveSpeedChanged;                       
    public event Action<float> OnElevationChanged;                       
    public event Action<Vector3, Quaternion> OnSaveCheckpoint;
    public event Action<Vector3, Quaternion> OnSaveLastSafeCheckpoint;   
    public event Action<Quaternion> OnFacingChanged;                     
    public event Action OnCycleTargetRight;                              
    public event Action OnCycleTargetLeft;                               
    public event Action OnTargetsCleared;                                

    public void PlayerDamaged(DamageSource source, DamageType type)
    {
        OnPlayerDamaged?.Invoke(source, type);
    }

    public void ToggleTorch(bool isOn)
    {
        OnPlayerTorchToggled?.Invoke(isOn);
    }

    public void SwitchQuickSlot(int directionOrIndex)
    {
        OnPlayerQuickSlotSwitched?.Invoke(directionOrIndex);
    }

    public void RetryEncounter()
    {
        OnRetryEncounter?.Invoke();
    }

    public void RequestRespawn()
    {
        OnRequestRespawn?.Invoke();
    }

    public void EnterZone(ZoneType zone)
    {
        OnEnteredZone?.Invoke(zone);
    }

    public void MoveSpeedChanged(float newSpeed)
    {
        OnMoveSpeedChanged?.Invoke(newSpeed);
    }

    public void ElevationChanged(float newY)
    {
        OnElevationChanged?.Invoke(newY);
    }

    public void SaveCheckpoint(Vector3 position, Quaternion rotation)
    {
        OnSaveCheckpoint?.Invoke(position, rotation);
    }

    public void SaveLastSafeCheckpoint(Vector3 position, Quaternion rotation = default)
    {
        OnSaveLastSafeCheckpoint?.Invoke(position, rotation);
    }

    public void CycleTargetRight()
    {
        OnCycleTargetRight?.Invoke();
    }

    public void CycleTargetLeft()
    {
        OnCycleTargetLeft?.Invoke();
    }

    public void FacingChanged(Quaternion rotation)
    {
        OnFacingChanged?.Invoke(rotation);
    }

    public void ClearTargets()
    {
        OnTargetsCleared?.Invoke();
    }
}