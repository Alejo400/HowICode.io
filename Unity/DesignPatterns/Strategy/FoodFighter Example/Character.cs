using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] GameObject stateIndicator;
    public GameObject StateIndicator { get => stateIndicator; set => stateIndicator = value; }
    protected Rigidbody _rb;
    protected Animator _animator;
    protected bool canMove;

    public abstract void Attack();
    public abstract void Die();
    public abstract void Hit();

}
