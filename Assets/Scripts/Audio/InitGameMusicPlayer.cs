using UnityEngine;

public sealed class InitGameMusicPlayer : CrossSceneSingleInstance
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private GameMusicPlayer musicPlayer;

    protected override string UniqueTag => "Music";
    protected override void OnAwake() => musicPlayer.InitIfNeeded(musicSource);
}
