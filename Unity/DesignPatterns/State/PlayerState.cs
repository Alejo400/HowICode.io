using UnityEngine;

public class PlayerState : CharacterState
{
    public PlayerState(PlayerController player, string animName, AnimationType animationType) 
        : base(player, animName, animationType)
    {
    }

    public override void Execute()
    {
        HandleSpriteFlip();

        // Evaluar transiciones cuando no se está realizando una acción del estado actual
        if (!isDoing)
        {
            CheckTransitions();
        }
    }

    /// <summary>
    /// Orden de prioridad de chequeo
    /// </summary>
    protected virtual void CheckTransitions()
    {
        if (CheckHurtTransition())   return;
        if (CheckDefenseTransition()) return;
        if (CheckAttackTransition())  return;
        if (CheckDashTransition())    return;
        if (CheckJumpTransition())    return;
        if (CheckWalkTransition())    return;
        CheckIdleTransition();
    }

    // --- Chequeos individuales: true si ejecutan la transición ---
    protected virtual bool CheckHurtTransition()
    {
        if (player.IsBeignHurt)
        {
            player.stateMachine.TransitionTo(player.stateMachine.hurtState);
            return true;
        }
        return false;
    }

    protected virtual bool CheckDefenseTransition()
    {
        if (player.IsDefending && !player.IsDashing)
        {
            player.stateMachine.TransitionTo(player.stateMachine.defenseState);
            return true;
        }
        return false;
    }

    protected virtual bool CheckAttackTransition()
    {
        if (player.IsAttacking && !player.IsDefending && !player.IsDashing)
        {
            player.stateMachine.TransitionTo(player.stateMachine.attackState);
            return true;
        }
        return false;
    }

    protected virtual bool CheckDashTransition()
    {
        if (player.IsDashing && !player.IsDefending)
        {
            player.stateMachine.TransitionTo(player.stateMachine.dashState);
            return true;
        }
        return false;
    }

    protected virtual bool CheckWalkTransition()
    {
        if (player.IsGrounded && player.IsNotFighting && player.IsMoving)
        {
            player.stateMachine.TransitionTo(player.stateMachine.walkState);
            return true;
        }
        return false;
    }

    protected virtual bool CheckJumpTransition()
    {
        if (!player.IsGrounded && player.IsNotFighting)
        {
            player.stateMachine.TransitionTo(player.stateMachine.jumpState);
            return true;
        }
        return false;
    }

    protected virtual bool CheckIdleTransition()
    {
        if (player.IsGrounded && player.IsNotFighting && !player.IsMoving)
        {
            player.stateMachine.TransitionTo(player.stateMachine.idleState);
            return true;
        }
        return false;
    }

    protected void HandleSpriteFlip()
    {
        if (player.SpriteRendererP != null && player.IsMoving && player.CanMove)
        {
            player.SpriteRendererP.flipX = player.HorizontalInput < 0;
        }
    }
}
