using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RepairSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] repair;
    [SerializeField] private float timeBetweenHits = .8f;

    private float _cooldownRemaining;
    private bool _isRepairing = false;
    private int index;
    private AudioSource audioSource;

    void Awake() => audioSource = GetComponent<AudioSource>();

    public void StopRepairing() => _isRepairing = false;
    public void StartRepairing() => _isRepairing = true;

    private void FixedUpdate()
    {
        _cooldownRemaining = Mathf.Max(0, _cooldownRemaining - Time.deltaTime);
        if (!_isRepairing)
            index = 0;
        if (!_isRepairing || _cooldownRemaining > 0)
            return;

        _cooldownRemaining = timeBetweenHits;
        index = (index + 1) % repair.Length;
        var repairSound = repair[index];
        audioSource.PlayOneShot(repairSound);
    }
}
