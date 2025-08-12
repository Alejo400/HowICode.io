using Fusion;
using UnityEngine;

public class PlayerDash : NetworkBehaviour
{    
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashDuration;
    [SerializeField]
    private float dashCooldown;

    private PlayerController player;
    [Networked] private TickTimer DashActiveTimer { get; set; }
    [Networked] private TickTimer CooldownTimer { get; set; }

    void Awake()
    {
        player = GetComponent<PlayerController>();
    }
    /// <summary>
    /// Logica para iniciar o esperar por el dash
    /// </summary>
    public void HandleDash(NetworkBool DashInput)
    {
        //Comprobamos si desea realizar el dash, tenemos cooldown y no estamos dashing
        if (DashInput && CooldownTimer.ExpiredOrNotRunning(Runner) && player.IsGrounded
            && player.IsNotFighting)
        {
            player.IsDashing = true;
            DashActiveTimer = TickTimer.CreateFromSeconds(Runner, dashDuration);
            PlayerEventManager._instance.StartPlayerDash();
        }
        //Gestionamos el tiempo del dashing y marcamos el inicio del timer tras su finalizacion
        if (player.IsDashing)
        {
            if (DashActiveTimer.IsRunning)
                Dash();
            else
                StopDash();
                
            player.CanMove = !player.IsDashing;
        }
    }

    private void Dash()
    {
        float direction = player.SpriteRendererP.flipX ? -1 : 1;
        player.RigidBody.linearVelocity = new Vector2(direction * dashSpeed * Runner.DeltaTime, 0);
    }
    private void StopDash()
    {
        player.IsDashing = false;
        // Iniciar el temporizador de cooldown
        CooldownTimer = TickTimer.CreateFromSeconds(Runner, dashCooldown);
    }
}
