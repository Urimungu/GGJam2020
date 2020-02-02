using UnityEngine;

public sealed class PlayerSounds : OnMessage<PlayerStartedWalking, PlayerStoppedWalking, PlayerJumped, PlayerDoubleJumped, 
    PlayerFalling, RepairCompleted, RepairStarted, RepairStopped, MissileHitPlayer>
{
    [SerializeField] private UiSfxPlayer uiSfx;
    [SerializeField] private AudioSource player;
    [SerializeField] private FootStepSound footsteps;
    [SerializeField] private RepairSounds repairing;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip airJump;
    [SerializeField] private AudioClip falling;
    [SerializeField] private AudioClip objectRepaired;
    [SerializeField] private AudioClip missile;

    protected override void Execute(PlayerStartedWalking msg) => footsteps.StartWalking();
    protected override void Execute(PlayerStoppedWalking msg) => footsteps.StopWalking();
    protected override void Execute(PlayerJumped msg) => player.PlayOneShot(jump);
    protected override void Execute(PlayerDoubleJumped msg) => player.PlayOneShot(airJump);
    protected override void Execute(PlayerFalling msg) => uiSfx.Play(falling);

    protected override void Execute(RepairStarted msg) => repairing.StartRepairing();
    protected override void Execute(RepairStopped msg) => repairing.StopRepairing();
    protected override void Execute(RepairCompleted msg)
    {
        repairing.StopRepairing();
        uiSfx.Play(objectRepaired);
    }

    protected override void Execute(MissileHitPlayer msg) => uiSfx.Play(missile);
}
