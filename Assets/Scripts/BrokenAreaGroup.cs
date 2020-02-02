using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenAreaGroup : MonoBehaviour
{
    /*This is how BrokenAreaGroup is going to work.
     This will take a gameObject in which has a lot of spawners as the child of that object.
     Once this object wakes, it'll key the child of the current object that it's attached to.
     If a Broken Area Spawner is deemed Irreparable, it'll lose that amount of hp.
     
    The HP of the BrokenAreaGroup will always be 100%, however, with fewer spawners means that it's easy to lose a lot
    of HP. HP is the fraction of how many spawners are in that group. For example, if a BrokenAreaGroup counts 4 Spawns as a child, each child is work
    100 / 4 (25hp). This will continously keep track of the count until it reaches 0, deeming the entire BrokenAreaGroup as Irreparable.

    That's at least the plan that I'm going for.*/

    //Current Health
    [Header("Current Health"), SerializeField]
    private float currentHealth;

    [Header("Irreparable?"), SerializeField]
    private bool irreparable;

    //Our maxHealth will always be 100
    private const float maxHealth = 100f; 

    //HealthUpdate Coroutine
    private IEnumerator healthUpdateRoutine;

    //If toggled, health will be recalculated
    private bool updateCount = false;

    //Divide HP after first update
    private float dividedHP;
    private bool completedFirstUpdate = false;

    //Create a list so that when iterating, we add all spawners that are still reparable
    [Header("Reparable Spawners"), SerializeField]
    private List<BrokenAreaSpawner> reparableSpawners;

    // Start is called before the first frame update
    void Start()
    {
        updateCount = true;
        healthUpdateRoutine = HealthUpdate();
        StartCoroutine(healthUpdateRoutine);
    }

    private IEnumerator HealthUpdate()
    {
        while (true)
        {
            //Calculate health
            if (updateCount)
            {
                
                CalculateTotalHealth();
                updateCount = false;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void CalculateTotalHealth()
    {
        //Get all existing spawner for us to iterate
        BrokenAreaSpawner[] detectedSpawners = BrokenSpawnerManager.Instance.GrabAllBrokenAreaSpawners().ToArray();

        reparableSpawners.Clear();
        reparableSpawners = new List<BrokenAreaSpawner>();

        //Check for spawners that are not marked irreparable
        foreach (BrokenAreaSpawner spawner in detectedSpawners)
        {
            if (spawner.GetSeverityLevelAsString() != "IRREPARABLE")
                reparableSpawners.Add(spawner);
        }

        if (completedFirstUpdate == false)
        {
            dividedHP = maxHealth / detectedSpawners.Length;
            completedFirstUpdate = true;
            Debug.Log("Fist update has ran successfully.");
        }

        currentHealth = reparableSpawners.Count * dividedHP;
    }

    public void SignalCountUpdate()
    {
        updateCount = true;
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}
