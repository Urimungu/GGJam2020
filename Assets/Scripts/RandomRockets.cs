using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockets : MonoBehaviour
{

    [SerializeField] private float missileFrequency;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject missile;
    [SerializeField] private Transform target;

    private void Start()
    {
        player = GameObject.Find("Player");
        target = player.GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            Instantiate(missile, target.position, target.rotation);
        }
    }


}
