using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class EnemySpawner : MonoBehaviour
{
    //Enemies that are Ready to be spawned
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> ActiveEnemies = new List<GameObject>();

    //References
    private GameObject EnemyPrefab;
    private GameObject Player;

    //Variables
    public float StartCoolDown = 5;
    public float spawnHeight = 5;
    public float spawnFrequency = 8;
    public int MaxEnemies = 5;
    private int CurrentEnemies;
    public bool CanSpawn;

    //Misc
    private float Timer;

    void Start() {
        //Gives the player a starting grace period
        Timer = Time.time + StartCoolDown;
        EnemyPrefab = Resources.Load<GameObject>("Enemy");
    }

    private void Update() {
        if (CanSpawn)
            StartOperation();

    }

    private void StartOperation()
    {
        //Checks to make sure the game has a Game Manager and Player
        if (GameManager.Manager == null || GameManager.Manager.Player == null) 
            return;

        if (Time.time > Timer) {
            Timer = Time.time + spawnFrequency;

            //Fills in the Enemy List if it needs Enemies
            if(Enemies.Count + ActiveEnemies.Count < MaxEnemies) {
                for(int i = 0; i < MaxEnemies - (Enemies.Count + ActiveEnemies.Count); i++)
                    CreateEnemy();
            }

            //If there isn't max enemies on screen spawn one
            if(ActiveEnemies.Count < MaxEnemies && MaxEnemies > 0)
                SpawnEnemy();
        }
    }


    //Spawns in the enemy at near a player location using Pool
    private void SpawnEnemy() {
        Enemies[0].SetActive(true);
        Enemies[0].transform.position = GameManager.Manager.Player.transform.position + new Vector3(0 ,spawnHeight,0);
        Enemies[0].GetComponent<EnemyMovement>().State = GameManager.Manager.Player.GetComponent<CharacterController>().GetState();
        ActiveEnemies.Add(Enemies[0]);
        Enemies.Remove(Enemies[0]);
    }

    //Creates a new enemy if there isn't one and pools it
    private void CreateEnemy() {
        GameObject shot = Instantiate(EnemyPrefab, transform.position, Quaternion.identity, transform);
        shot.SetActive(false);
        Enemies.Add(shot);
    }

    //When the Enemy Dies it needs to be pooled
    public void PoolEnemy(GameObject enemy) {
        Enemies.Add(enemy);
        ActiveEnemies.Remove(enemy);
    }

    public void StopSpawningEnemies(bool ClearField = false) {
        CanSpawn = false;
        //If they want to stop spawning enemies and want to clear the game of them
        if(ClearField)
            ClearEnemies();
    }

    public void StartSpawningEnemies() {


    }

    //Clears all of the Enemies from the game
    public void ClearEnemies() {
        //Pools every active enemy in the game
        for (int i = 0; i < ActiveEnemies.Count; i++){
            ActiveEnemies[ActiveEnemies.Count - i].SetActive(false);
            Enemies.Add(ActiveEnemies[ActiveEnemies.Count - i]);
            ActiveEnemies.Remove(ActiveEnemies[ActiveEnemies.Count - i]);
        }
    }

}
