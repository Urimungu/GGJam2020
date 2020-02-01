using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof (AudioSource))]
public class FootStepSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] steps;
    [SerializeField] private float timeBetweenSteps = 0.3f;

    private float _cooldownRemaining;
    private bool _isWalking = true;
    private int index;
    private AudioSource characterSteps; 
 
    void Awake()
    {
        characterSteps = GetComponent<AudioSource>();
    }

  
    private void FixedUpdate()
    {
        _cooldownRemaining = Mathf.Max(0, _cooldownRemaining - Time.deltaTime);
        if (!_isWalking || _cooldownRemaining > 0)
            return;

        _cooldownRemaining = timeBetweenSteps;
        index = (index + 1) % steps.Length;
        var stepSound = steps[index];
        characterSteps.PlayOneShot(stepSound);
        Debug.Log("FootStep");
    }
}
