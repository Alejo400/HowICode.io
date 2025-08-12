using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player, "Idle", AnimationType.Bool)
    {
    }

    public override void Execute()
    {
        base.Execute();
    }
}
