using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class FootStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] steps;
    [SerializeField] private float timeBetweenSteps = 0.3f;
    [SerializeField] private bool startWalkingOnAwake = true;

    private float _cooldownRemaining;
    private bool _isWalking = true;
    private int index;
    private AudioSource characterSteps; 
 
    void Awake()
    {
        characterSteps = GetComponent<AudioSource>();
        _isWalking = startWalkingOnAwake;
    }

    public void StartWalking()
    {
        _isWalking = true;
    }

    public void StopWalking() => _isWalking = false;
    
    private void FixedUpdate()
    {
        _cooldownRemaining = Mathf.Max(0, _cooldownRemaining - Time.deltaTime);
        if (!_isWalking || _cooldownRemaining > 0)
            return;

        _cooldownRemaining = timeBetweenSteps;
        index = (index + 1) % steps.Length;
        var stepSound = steps[index];
        characterSteps.PlayOneShot(stepSound);
    }
}
