using System.Collections;
using UnityEngine;

public abstract class Ability<T> : MonoBehaviour
{
    public string AbilityName;
    public float cooldown;
    public float freezeCooldown;
    public GameObject ActionIcon;
    protected bool canUseAbility;
    public bool CanUseAbility { get => canUseAbility; set => canUseAbility = value; }
    [SerializeField] AnimationClip animDuration;
    public AnimationClip AnimDuration { get => animDuration; set => animDuration = value; }
    public abstract void ExecuteAbility(T _target, Character character);
    public abstract IEnumerator AllowUseAbility(); 
}
