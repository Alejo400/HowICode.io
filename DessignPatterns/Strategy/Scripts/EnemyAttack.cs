using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using EnemyBullet;
using System.Collections.Generic;

public class EnemyAttack
{
    protected List<Coroutine> CoroutineAbility;
    [SerializeField] protected List<EnemyAbility<GameObject>> Abilities 
                                        = new List<EnemyAbility<GameObject>>();
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
            CoroutineAbility = new List<Coroutine>(Abilities.Count);
            ExecuteAbilities();
        }
    }
    /// <summary>
    /// Stop all abilities of enemy (coroutine list)
    /// </summary>
    void StopAbilities(){
        if(_enemyFollow && CoroutineAbility != null){
            foreach (var item in CoroutineAbility)
            {
                StopAbilites(item);   
            }
        }
    }
    /// <summary>
    /// Init all abilities of enemy
    /// </summary>
    protected virtual void ExecuteAbilities(){
        CoroutineAbility.Clear();
        foreach (var item in Abilities)
        {
            if(item != null){
                Coroutine coroutine = StartCoroutine(item.ExecuteAbility(_enemyData, _player, gameObject));
                CoroutineAbility.Add(coroutine);
            }
        }
    }
    protected virtual void StopAbilites(Coroutine coroutine){
        StopCoroutine(coroutine);
        coroutine = null;
    }
}
