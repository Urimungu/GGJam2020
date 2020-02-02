using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosion : MonoBehaviour
{
    [SerializeField] private float timer = 1f;
    private float createTime = 0.0f;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject rocket;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private Animator cameraAnim;
    bool inRange;

    private void Awake()
    {
        player = GameManager.Manager.Player;
        inRange = false;
       // Instantiate(rocket, transform.position, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("MainCamera").transform.position - transform.position));
    }

    private void Start()
    {
        cameraAnim = Camera.main.GetComponent<Animator>();
        createTime = Time.time;
    }
    private void Update()
    {    
        if ((Time.time - createTime) > timer)
        {
            //if player is not
            Message.Publish(new MissileMissedPlayer());
            Instantiate(explosion, transform.position, Quaternion.LookRotation(GameObject.FindGameObjectWithTag("MainCamera").transform.position - transform.position));
            cameraAnim.SetTrigger("Shake");
            Destroy(gameObject);
            //if player is in explosion radius, die
            if (inRange)
            {
                GameManager.Manager.KillPlayer();
            }
        }
    }


    //entering explosion radius
    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player(Clone)")
        {
            inRange = true;

        }
        else { inRange = false; }
    }

    //Leaving explosion radius
    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player(Clone)")
        {
            inRange = false;
        }
    }
}
