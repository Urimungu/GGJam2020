using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    [SerializeField] private float timer = 1f;
    private float createTime = 0.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private ParticleSystem explosion;

    private void Start()
    {
        player = GameObject.Find("Player");
        createTime = Time.time;
    }
    private void Update()
    {

        if ((Time.time - createTime) > timer)
        {
            Instantiate(explosion, transform.position, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("MainCamera").transform.position - transform.position));
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((Time.time - createTime) > timer && other.name == "Player")
        {
            Destroy(player);
        }
    }

}
