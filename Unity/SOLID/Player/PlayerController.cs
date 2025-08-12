using Fusion;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerJump))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerHurt))]
public class PlayerController : NetworkBehaviour, IPlayerLeft
{
    //State Machine -------------------------------------------------
    public StateMachine stateMachine { get; set; }
    //Stats variables -------------------------------------------------
    int modeAttack;
    public int ModeAttack { get => modeAttack; set => modeAttack = value; }
    [SerializeField] float moveSpeed, jumpForce, minWalkSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float JumpForce { get => jumpForce; set => jumpForce = value; }
    public float MinWalkSpeed { get => minWalkSpeed; set => minWalkSpeed = value; }
    [SerializeField]
    bool isGrounded, isMoving, isDashing, canMove = true, isAttacking, isDefending, isBeignHurt;
    public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
    public bool CanMove { get => canMove; set => canMove = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public bool IsDefending { get => isDefending; set => isDefending = value; }
    public bool IsBeignHurt { get => isBeignHurt; set => isBeignHurt = value; }
    //Inputs utilizados en otros scripts como el PlayerState
    float horizontalInput;
    public float HorizontalInput { get => horizontalInput; set => horizontalInput = value; }
    //Components -------------------------------------------------
    //Unity Native Components
    Rigidbody2D _rigidbody;
    public Rigidbody2D RigidBody => _rigidbody; 
    Animator animator;
    public Animator AnimatorP => animator;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRendererP => spriteRenderer; 
    //Player Actions Components
    PlayerMovement playerMovement;
    PlayerJump playerJump;
    PlayerDash playerDash;
    PlayerAttack playerAttack;
    PlayerDefense playerDefense;
    PlayerHurt playerHurt;
    public PlayerMovement PlayerMovementP => playerMovement;
    public PlayerJump PlayerJumpP => playerJump;
    public PlayerDash PlayerDashP => playerDash;
    public PlayerAttack PlayerAttackP => playerAttack;
    public PlayerDefense PlayerDefenseP => playerDefense;
    public PlayerHurt PlayerHurtP => playerHurt;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        playerDash = GetComponent<PlayerDash>();
        playerAttack = GetComponent<PlayerAttack>();
        playerDefense = GetComponent<PlayerDefense>();
        playerHurt = GetComponent<PlayerHurt>();

        stateMachine = new StateMachine(this);
    }
    private void Start()
    {
        stateMachine.Initialize(stateMachine.idleState);
    }
    private void Update()
    {
        stateMachine.Execute();
    }
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority && !HasInputAuthority) 
           return; 
        
        if(GetInput(out NetworkInputData data)){
            horizontalInput = data.HorizontalInput;

            playerMovement.Move(data.HorizontalInput);
            playerJump.Jump(data.JumpInput);
            playerDash.HandleDash(data.DashInput);
            playerAttack.Attack(data.Attack1Input, data.Attack2Input, data.SpecialAttackInput);
            playerDefense.Defense(data.DefenseInput, data.DefenseUpInput);
        }
    }
    public bool IsNotFighting => !isDashing && !isAttacking && !isDefending && !isBeignHurt;
    /// <summary>
    /// Resetear los estados activos cuando pasemos a una anim desde AnyState
    /// </summary>
    public void ResetFlagsOnAnyState(){
        isAttacking = false;
        isDashing = false;
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
        }
    }
}
