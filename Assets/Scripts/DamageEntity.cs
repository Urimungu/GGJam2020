using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEntity : MonoBehaviour
{

    /*Damage Entity will be responsible for the random spawning of damaged areas.
     Now that we got everything set up and ready to go, this shouldn't be a problem for any of us.
     
    This class has a min duration variable, as well as a max duration variable.
    A random duration will be set depending on the range of it. Each spawn, the max and min range will decrease,
    while keeping the same range.
         */

    [Header("Min Duration Value"), Range(1, 100)]
    [SerializeField] private int minDurationValue = 10; //10 seconds is default

    [Header("Max Duration Value"), Range(1, 100)]
    [SerializeField] private int maxDurationValue = 60; //A minute (60 seconds) is default

    [Header("Decrement Value"), Range(1, 10)]
    [SerializeField] private int decrementValue = 1; //Range decrease every spawn

    [Header("Decrement Limit")]
    [SerializeField] private float decrementLimit = 1;

    //A bool to tell us that this script will not run
    private bool cantExecuteScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// The decrementLimit can not be greater than
    /// the minimum duration range, otherwise, we would have a problem.
    /// </summary>
    [ExecuteAlways]
    void CheckDecrementLimit()
    {

    }
}
