using UnityEngine;

public class Player : Character
{
    PlayerAttack _playerAttack;
    void Start(){
        _playerAttack = GetComponent<PlayerAttack>();
    }
    void Update(){
        Attack();
    }
    public override void Attack()
    {
        if(!_playerAttack.IAttacking && (GameManager._SharedIntance._gameStatus == GameStatus.InGame
        || GameManager._SharedIntance._gameStatus == GameStatus.StartingGame)){
            //Habilidad de empujar
            if(Input.GetKeyDown(KeyCode.Return)){ 
                _playerAttack.CallAbility(
                "PlayerPushAbility", SoundManager._SharedInstance._PlayerSounds.attack, "isAttacking", 4); 
            }

            //Habilidad especial silbon
            if(Input.GetKeyDown(KeyCode.E)){ 
                _playerAttack.CallAbility(
                "SilbonAbility", SoundManager._SharedInstance._PlayerSounds.invoke, "isInvoking", 2); 
            }
        }
    }
}
