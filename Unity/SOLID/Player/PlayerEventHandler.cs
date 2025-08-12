using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    PlayerController player;
    void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    void Start()
    {
        PlayerEventStartHandler();
    }
    void OnDisable()
    {
        PlayerEventEndHandler();
    }
    void PlayerEventStartHandler(){
        PlayerEventManager._instance.OnStopPlayer += player.PlayerMovementP.StopMovement;
        PlayerEventManager._instance.OnEndStopPlayer += player.PlayerMovementP.AllowMovement;
        //Defense
        PlayerEventManager._instance.OnStartPlayerDefense += player.PlayerMovementP.StopMovement;
        PlayerEventManager._instance.OnEndPlayerDefense += player.PlayerMovementP.AllowMovement;
        //Hurt
        PlayerEventManager._instance.OnStartPlayerHurt += player.PlayerHurtP.Hurt;
        PlayerEventManager._instance.OnStartPlayerHurt += player.PlayerMovementP.StopMovement;
        PlayerEventManager._instance.OnEndPlayerHurt += player.PlayerMovementP.AllowMovement;
    }
    void PlayerEventEndHandler(){
        PlayerEventManager._instance.OnStopPlayer -= player.PlayerMovementP.StopMovement;
        PlayerEventManager._instance.OnEndStopPlayer -= player.PlayerMovementP.AllowMovement;
        //Defense
        PlayerEventManager._instance.OnStartPlayerDefense -= player.PlayerMovementP.StopMovement;
        PlayerEventManager._instance.OnEndPlayerDefense -= player.PlayerMovementP.AllowMovement;
        //Hurt
        PlayerEventManager._instance.OnStartPlayerHurt -= player.PlayerHurtP.Hurt;
        PlayerEventManager._instance.OnStartPlayerHurt -= player.PlayerMovementP.StopMovement;
        PlayerEventManager._instance.OnEndPlayerHurt -= player.PlayerMovementP.AllowMovement;
    }
}
