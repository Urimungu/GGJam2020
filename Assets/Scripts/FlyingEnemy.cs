using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector3 playerPos;
    private Vector3 newPos;
    public float maxDistance = 10.0f;
    public float minDistance = 5.0f;
    private Transform Player;

    void Start()
    {
        Player = GameManager.Manager.Player.transform;
    }

    void Update()
    {
        playerPos = GameManager.Manager.Player.transform.position;

        transform.LookAt(Player.transform);

        if (Vector3.Distance(transform.position, Player.position) >= minDistance)
        {

            transform.position += transform.forward * speed * Time.deltaTime;



            if (Vector3.Distance(transform.position, Player.position) <= maxDistance)
            {
                //Here Call any function U want Like Shoot at here or something
            }
        }
    }
}