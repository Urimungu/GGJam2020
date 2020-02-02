using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLaunch : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 target;
    public GameObject player;
    public float speed = 2f;
    // Use this for initialization
    void Start()
    {
        player = GameManager.Manager.Player;
        target = player.transform.position;
        direction = Vector3.MoveTowards(transform.position, target, speed);

    }

    // Update is called once per frame
    void Update()
    {
        print("Rocket sees " + player);
        transform.position = transform.position + direction;
        //if(transform.position == target)
        //{
        //    Destroy(gameObject);
        //}

    }
}
