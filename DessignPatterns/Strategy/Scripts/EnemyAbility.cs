using System.Collections;
using UnityEngine;

public abstract class EnemyAbility<T> : ScriptableObject
{
    public string AbilityName;
    public abstract IEnumerator ExecuteAbility(EnemyData _EnemyData, T _Target, GameObject gameObject);
}
