using UnityEngine;

public sealed class PlayerSounds : OnMessage<PlayerStartedWalking, PlayerStoppedWalking, PlayerJumped, PlayerDoubleJumped, PlayerFalling>
{
    [SerializeField] private AudioSource player;
    [SerializeField] private FootStepSound footsteps;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip airJump;
    [SerializeField] private AudioClip falling;

    protected override void Execute(PlayerStartedWalking msg) => footsteps.StartWalking();
    protected override void Execute(PlayerStoppedWalking msg) => footsteps.StopWalking();
    protected override void Execute(PlayerJumped msg) => player.PlayOneShot(jump);
    protected override void Execute(PlayerDoubleJumped msg) => player.PlayOneShot(airJump);
    protected override void Execute(PlayerFalling msg) => player.PlayOneShot(falling);
}
