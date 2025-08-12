using System;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager _instance;

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

    public event Action OnStartNewQuest;                 
    public event Action<GamePhase> OnGamePhaseChanged;   
    public event Action OnStopAllRoutines;               
    public event Action OnReputationLost;                
    public event Action OnGameCompleted;                 

    public void StartNewQuest()
    {
        OnStartNewQuest?.Invoke();
    }

    public void ChangeGamePhase(GamePhase phase)
    {
        OnGamePhaseChanged?.Invoke(phase);
    }

    public void StopAllRoutines()
    {
        OnStopAllRoutines?.Invoke();
    }

    public void LoseReputation()
    {
        OnReputationLost?.Invoke();
    }

    public void CompleteGame()
    {
        OnGameCompleted?.Invoke();
    }
}
