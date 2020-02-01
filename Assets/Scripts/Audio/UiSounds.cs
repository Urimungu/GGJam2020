using UnityEngine;

public class UiSounds : OnMessage<UiConfirmed, UiCancelled, UiSelectionChanged>
{
    public UiSfxPlayer player;
    public AudioClip confirm;
    public AudioClip cancel;
    public AudioClip selectionChanged;

    protected override void Execute(UiConfirmed msg) => player.Play(confirm);
    protected override void Execute(UiCancelled msg) => player.Play(cancel);
    protected override void Execute(UiSelectionChanged msg) => player.Play(selectionChanged, 0.6f);
}
