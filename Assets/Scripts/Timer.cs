using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    //We need minutes and seconds to be our global values
    private float minutes = 0f;
    private float seconds = 0.00f;

    //Coroutine variable
    private IEnumerator timerSystemRoutine;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Set up and start coroutine
        timerSystemRoutine = RunTimerSystem();
        StartCoroutine(CountDownSeconds());
        StartCoroutine(timerSystemRoutine);
    }

    private IEnumerator RunTimerSystem()
    {
        while (true)
        {

            if (seconds > 59.99f)
            {
                seconds = 0f;
                minutes++;
            }
            UIManager.Instance.SetTimerText(minutes, seconds);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator CountDownSeconds()
    {
        while (true)
        {
            const float millisecond = 0.001f;
            yield return new WaitForSeconds(millisecond);
            seconds += 1/60f;
        }
    }
}
