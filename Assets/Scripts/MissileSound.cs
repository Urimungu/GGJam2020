using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MissileSound : OnMessage<MissileDetonated>
{
    [SerializeField] private UiSfxPlayer uiSfx;
    [SerializeField] private AudioClip launch;
    [SerializeField] private AudioClip explode;
    [SerializeField] private float launchVolume;

    private AudioSource audioSource;
   


    protected override void Execute(MissileDetonated msg) => uiSfx.Play(explode);
    void Awake() => audioSource = GetComponent<AudioSource>();
    private void Start()
    {
            audioSource.PlayOneShot(launch, launchVolume);
}

}
