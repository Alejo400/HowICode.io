using System;
using System.Diagnostics;

[Serializable]
public class StateMachine
{
    public IState CurrentState { get; private set; }
    public WalkState walkState;
    public JumpState jumpState;
    public IdleState idleState;
    public DashState dashState;
    public AttackState attackState;
    public DefenseState defenseState;
    public HurtState hurtState;
    public CharacterState characterState;

    public StateMachine(PlayerController player)
    {
        this.walkState = new WalkState(player);
        this.jumpState = new JumpState(player);
        this.idleState = new IdleState(player);
        this.dashState = new DashState(player);
        this.attackState = new AttackState(player);
        this.defenseState = new DefenseState(player);
        this.hurtState = new HurtState(player);
    }
    public void Initialize(IState startingState)
    {
        CurrentState = startingState ?? throw new ArgumentNullException(nameof(startingState));
        startingState.Enter();
    }
    public void TransitionTo(IState nextState)
    {
        if(nextState != CurrentState){
            CurrentState?.Exit();
            CurrentState = nextState ?? throw new ArgumentNullException(nameof(nextState));
            nextState.Enter();
        }
    }
    public void Execute()
    {
        CurrentState?.Execute();
    }
}
