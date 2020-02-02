using System.Collections;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class FootStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] steps;
    [SerializeField] private float timeBetweenSteps = 0.3f;
    [SerializeField] private bool startWalkingOnAwake = true;
    [SerializeField] private float delayBeforeCanWalk = 1.5f;
    
    private float _cooldownRemaining;
    private bool _isWalking = false;
    private int index;
    private AudioSource characterSteps; 
 
    void Awake() => characterSteps = GetComponent<AudioSource>();
    void Start() => StartCoroutine(StartAutoWalk());
    

    private IEnumerator StartAutoWalk()
    {
        if (!startWalkingOnAwake)
            yield break;
        yield return new WaitForSeconds(delayBeforeCanWalk);
        _isWalking = startWalkingOnAwake;
    }

    public void StopWalking() => _isWalking = false;
    public void StartWalking() => _isWalking = true;
    
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
