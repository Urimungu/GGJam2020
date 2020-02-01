using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    public static LoseCondition Instance;

    //This keeps track of the number of broken areas. If it hits higher than the max, you lose the game;
    [Header("Losing Condition"), SerializeField]
    private int maxBrokenAreas = 30; //Default

    [Header("Counter"), SerializeField]
    private int amountOfBrokenAreas = 0;

    //A flag to update counter
    private bool updateCounter;

    //A flag to state that you lost
    private bool lost;

    //UpdateCounter Coroutines
    private IEnumerator updateCounterRoutine;
    private IEnumerator checkLostRoutine;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        updateCounterRoutine = CounterUpdate();
        checkLostRoutine = CheckIfLost();

        StartCoroutine(updateCounterRoutine);
        StartCoroutine(checkLostRoutine);
    }

    private IEnumerator CounterUpdate()
    {
        while (true)
        {
            if (updateCounter)
            {
                BrokenArea[] visibleBrokenAreas = FindObjectsOfType<BrokenArea>();
                amountOfBrokenAreas = visibleBrokenAreas.Length;
                updateCounter = false;

                //Check if amountOfBrokenArea is higher than max
                if (amountOfBrokenAreas >= maxBrokenAreas)
                {
                    lost = true;
                    StopCoroutine(updateCounterRoutine);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CheckIfLost()
    {
        while (true)
        {
            if (lost)
            {
                Debug.Log("GAMEOVER!!!");
                StopCoroutine(checkLostRoutine);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void SendSignalToUpdateCounter()
    {
        updateCounter = true;
    }
}
