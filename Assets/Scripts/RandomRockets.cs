using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockets : MonoBehaviour
{
    //Spawn this object
    public GameObject spawnObject;

    public float maxTime = 5;
    public float minTime = 2;

    //current time
    private float time;

    //The time to spawn the object
    private float spawnTime;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform target;

    void Start()
    {
        SetRandomTime();
        time = minTime;
        
    }

    void FixedUpdate()
    {
        player = GameManager.Manager.Player;
        target = player.GetComponent<Transform>();
        //Counts up
        time += Time.deltaTime;

        //Check if its the right time to spawn the object
        if (time >= spawnTime)
        {
            SpawnObject();
            SetRandomTime();
        }

    }


    //Spawns the object and resets the time
    void SpawnObject()
    {
        time = 0f;
        Instantiate(missile, player.transform.position, target.transform.rotation);
    }

    //Sets the random time between minTime and maxTime
    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

}



