using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class EnemyController : MonoBehaviour
{
    //Variables
    public float Health;
    public float PitLength = 4.2f;  //Distance of Gap he can Jump
    public float JumpToDistance;
    public float FallDistance = 4;

    public float DeathOnNonMatch = 6;
    public bool DeathComing;  //Kills the enemy if the bool isn't reset in time
    private float Horizontal;

    private Vector3 RightDirection;
    private int oldState = 4;
    private bool isRunning;
    private bool StopMoving;

    //References
    private Transform Player;
    private EnemyMovement Move;

    void Update() {
        //Makes sure there is a player
        if (GameManager.Manager.Player == null)
            return;
        //Makes sure the player isn't missing
        if (Player == null) 
            Player = GameManager.Manager.Player.transform;

        //Controls the Enemy
        Move = GetComponent<EnemyMovement>();
        Move.ControlTheEnemy(Movement(), DecideJump());

        if (oldState != Move.State) {
            RightDirection = Move.Direction();
            oldState = Move.State;
        }

        if(Player.GetComponent<CharacterController>().GetState() != Move.State) {
            isRunning = true;
            if(!isRunning)
                StartCoroutine(DeathTimer());
        } else
            StopCoroutine(DeathTimer());

        Horizontal = Move.Horizontal;

    }

    private float Movement() {
        Vector3 spawn = (transform.position + GetComponent<CapsuleCollider>().center);
        if (!Physics.Raycast(spawn, Vector3.down, 5, Move.layerMask)) {
            StopMoving = true;
            return Horizontal;
        }

        if (StopMoving && !Move.CheckGrounded()) 
            return 0;
        else 
            StopMoving = false;


        if(distance() > 0.3f) 
            return 1;
        

        if(distance() < -0.3f){
            return -1;
        }
        return 0;
    }

    public float distance() {
        switch(Move.State) {
            case 0: //Front
                return -(transform.position.x - Player.transform.position.x);
            case 1: //Right
                return -(transform.position.z - Player.transform.position.z);
            case 2: //Back
                return (transform.position.x - Player.transform.position.x);
            case 3: //Left
                return (transform.position.x - Player.transform.position.x);
        }

        return 0;
    }

    private bool DecideJump() {
        //Positive (Moves Right)
        Vector3 spawn = (transform.position + GetComponent<CapsuleCollider>().center);

        //Shoots the Raycasts
        bool hit1Hit = Physics.Raycast(spawn + (RightDirection * 0.5f), Vector3.down, 5, Move.layerMask);
        bool hit2Hit = Physics.Raycast(spawn + (RightDirection * PitLength), Vector3.down, 5, Move.layerMask);

        //Should Jump
        if (!hit1Hit && hit2Hit)
            return true;

        Debug.DrawRay(spawn + (RightDirection * PitLength * Horizontal), Vector3.down, Color.red);
        Debug.DrawRay(spawn + (RightDirection * 0.5f * Horizontal), Vector3.down, Color.red);
        return false;
    }

    IEnumerator DeathTimer() {
        yield return new WaitForSeconds(DeathOnNonMatch);
        if(DeathComing)
            KillEnemy();

    }

    public void DealDamage(float damage) {
        Health -= damage;
        //If enemy falls then die
        if (Health <= 0)
            KillEnemy();
    }

    public void KillEnemy() {
        //If there is no SpawnManager then Destroy this
        if (transform.parent == null || transform.parent.GetComponent<EnemySpawner>() == null) {
            Destroy(gameObject);
            return;
        }

        //Returns the Enemy to the Spawn Manager
        transform.parent.GetComponent<EnemySpawner>().PoolEnemy(gameObject);

    }

    //Death on Fall
    void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("DeathZone"))
            KillEnemy();
    }
}
