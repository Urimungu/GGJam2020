using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MissileSound : OnMessage<MissileLaunched,MissileDetonated>
{
    [SerializeField] private AudioClip launch;
    [SerializeField] private AudioClip explode;
   
    private AudioSource audioSource;

    protected override void Execute(MissileLaunched msg) => audioSource.PlayOneShot(launch);
    protected override void Execute(MissileDetonated msg) => audioSource.PlayOneShot(explode);
    void Awake() => audioSource = GetComponent<AudioSource>();
    
}
