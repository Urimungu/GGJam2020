using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Variables
    public float Health;
    public float PitLength = 4.2f;  //Distance of Gap he can Jump
    public float JumpToDistance;

    public float DeathOnNonMatch = 6;
    public bool DeathComing;  //Kills the enemy if the bool isn't reset in time
    private float Horizontal;

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
        Move.ControlTheEnemy(Movement(), DecideJump());
    }

    private float Movement() {

        return 0;
    }

    private bool DecideJump() {
        //Positive (Moves Right)
        if(Horizontal > 0) {


        } else if (Horizontal < 0) {


        }
        return false;
    }

    IEnumerator DeathTimer() {
        yield return new WaitForSeconds(DeathOnNonMatch);


    }
    private void ChasePlayer() {


    }


    //Pools the Object
    public void Death() {


    }

    public void DealDamage(float damage) {
        Health -= damage;
        //If enemy falls then die
        if (Health <= 0)
            KillEnemy();
    }

    public void KillEnemy() {
        //If there is no SpawnManager then Destroy this
        if(transform.parent.GetComponent<EnemySpawner>() == null)
            Destroy(gameObject);

        //Returns the Enemy to the Spawn Manager
        transform.parent.GetComponent<EnemySpawner>().PoolEnemy(gameObject);

    }
}
