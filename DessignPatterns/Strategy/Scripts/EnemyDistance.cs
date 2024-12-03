using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using EnemyBullet;

public class EnemyDistance : Enemy
{
    protected Coroutine CoroutineAbility;
    [SerializeField] protected EnemyAbility<GameObject> Ability;
    void OnEnable()
    {
        InitAbilities();
    }
    void OnDisable(){
        StopAbilities();
    }
    void InitAbilities(){
        MyStart();
        if(_enemyFollow && CoroutineAbility == null){
            ExecuteAbilities();
        }
    }
    void StopAbilities(){
        if(_enemyFollow && CoroutineAbility != null){
            StopAbilites(CoroutineAbility);
        }
    }
    protected virtual void ExecuteAbilities(){
        CoroutineAbility = StartCoroutine(
        Ability.ExecuteAbility(_enemyData,_player,gameObject));
    }
    protected virtual void StopAbilites(Coroutine coroutine){
        StopCoroutine(coroutine);
        coroutine = null;
    }
}
