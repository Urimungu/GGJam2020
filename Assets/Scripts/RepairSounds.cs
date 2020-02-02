using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class RepairSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] repair;
    [SerializeField] private float timeBetweenHits = 2f;

    private float _cooldownRemaining;
    private bool _isRepairing = true;
    private int index;
    private AudioSource repairHits;
    

    void Awake()
    {
        repairHits = GetComponent<AudioSource>();
    }


    private void FixedUpdate()
    {
        _cooldownRemaining = Mathf.Max(0, _cooldownRemaining - Time.deltaTime);
        if (!_isRepairing || _cooldownRemaining > 0)
            return;

        _cooldownRemaining = timeBetweenHits;
        index = (index + 1) % repair.Length;
        var repairSound = repair[index];
        repairHits.PlayOneShot(repairSound);
        Debug.Log("FootStep");
    }
}
