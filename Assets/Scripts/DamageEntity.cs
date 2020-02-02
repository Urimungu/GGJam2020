using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageEntity : MonoBehaviour
{

    /*Damage Entity will be responsible for the random spawning of damaged areas.
     Now that we got everything set up and ready to go, this shouldn't be a problem for any of us.
     
    This class has a min duration variable, as well as a max duration variable.
    A random duration will be set depending on the range of it. Each spawn, the max and min range will decrease,
    while keeping the same range.
         */

    [Header("Min Duration Value"), Range(0.25f, 100f)]
    [SerializeField] private float minDurationValue = 10; //10 seconds is default

    [Header("Max Duration Value"), Range(1, 100)]
    [SerializeField] private float maxDurationValue = 60; //A minute (60 seconds) is default

    [Header("Min Damage Value"), Range(1, 100)] [SerializeField]
    private int minDamageValue = 10;

    [Header("Max Damage Value"), Range(1, 100)] [SerializeField]
    private int maxDamageValue = 100;

    [Header("Decrement Value"), Range(1, 10)]
    [SerializeField] private int decrementValue = 1; //Range decrease every spawn

    [Header("Decrement Limit"), Range(0.25f, 10f)]
    [SerializeField] private float decrementLimit = 0.25f;

    //Have us a timer, and set duration
    private float time;
    private float setDuration;

    //Simulation Coroutine
    private IEnumerator simulationRoutine;

    //Reset constant
    private const uint reset = 0;

    // Start is called before the first frame update
    void Start()
    {
        simulationRoutine = SimulateTakingDamage();

        if (!Concerned())
            StartCoroutine(simulationRoutine);
        else
            Debug.Log("DecrementLimit must be smaller than minDurationValue. Simulation will not execute.");

    }

    private IEnumerator SimulateTakingDamage()
    {
        while (true)
        {
            time += Time.deltaTime;
            if (time > setDuration)
            {

                time = reset;

                //Get a random brokenSpawner, and apply damage;
                int randomSpawnerValue =
                    Random.Range((int)reset, BrokenSpawnerManager.Instance.GrabAllBrokenAreaSpawners().Count);

                //Get random damage amount
                int randomDamageValue = Random.Range(minDamageValue, maxDamageValue);

                BrokenSpawnerManager.GetAreaSpawnerByIndex(randomSpawnerValue).SetDamage(randomDamageValue);

                DecrementRange();
                RandomizeDuration();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// The decrementLimit can not be greater than
    /// the minimum duration range, otherwise, we would have a problem.
    /// </summary>
    bool Concerned()
    {
        return (decrementLimit > minDurationValue);
    }

    void RandomizeDuration()
    {
        setDuration = Random.Range(minDurationValue, maxDurationValue);
    }

    void DecrementRange()
    {
        if (minDurationValue > decrementLimit)
            minDurationValue -= decrementLimit;
        
        if (maxDurationValue > decrementLimit)
            maxDurationValue -= decrementLimit;
    }
}
