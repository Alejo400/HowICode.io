using UnityEngine;

public class DefenseState : PlayerState
{
    public DefenseState(PlayerController player) : base(player, "isDefending", AnimationType.Bool)
    {
    }

    public override void Enter()
    {
        base.Enter();
        PlayerEventManager._instance.StartPlayerDefense();
    }

    public override void Execute()
    {
        base.Execute();
    }
}
