﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Manager;

    //References
    public GameObject MainCam;
    public GameObject Player;
    public GameObject SpawnPoint;
    public float fallDurationSeconds = 4f;

    //Variables
    private bool Started = false;
    private bool SinglePlayer = true;
    private bool isRunning;

    //Navigation
    public Transform TopDoor, BottomDoor;
    public List<GameObject> TopBounds = new List<GameObject>();
    public List<GameObject> BottomBounds = new List<GameObject>();

    private void Awake(){
        //Makes this Game Manager a Singleton
        #region  Singleton
        if(Manager == null) {
            Manager = GetComponent<GameManager>();
            DontDestroyOnLoad(gameObject);
        }
        else //Destroys itself if there is already a Game Manager in the Scene
            Destroy(gameObject);
        #endregion
    }


    private void FixedUpdate() {
        //Checks to see if the Scene was Just loaded and a new game started
        if (SceneManager.GetActiveScene().name == "SinglePlayer" && !Started) {
            Started = true;
            SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
            GatherNavigation();
            Spawn();
        }

    }

    private void GatherNavigation() {
        //Finds the Bounds in the Scene
        TopBounds.Clear();
        BottomBounds.Clear();
        Transform temp = GameObject.FindGameObjectWithTag("Bounds").transform;

        //Fills in the Arrays
        for (int i = 0; i < 4; i++) {
            TopBounds.Add(temp.GetChild(0).GetChild(i).gameObject);
            BottomBounds.Add(temp.GetChild(1).GetChild(i).gameObject);
        }
        //Doors
        TopDoor = temp.GetChild(2).GetChild(0);
        BottomDoor = temp.GetChild(2).GetChild(1);

        //Sets the Player
        Player = Instantiate(Resources.Load<GameObject>("Player"), SpawnPoint.transform.position, Quaternion.identity);
        for(int i = 0; i < 4; i++)
            Player.GetComponent<CharacterController>().Bounds.Add(TopBounds[i].transform.position);


        //Camera
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").transform.gameObject;
        MainCam.transform.parent.GetComponent<CameraController>().Player = Player.transform;
        MainCam.transform.parent.GetComponent<CameraController>().enabled = true;
        MainCam.transform.parent.GetComponent<CameraController>().ChangeDir();
    }


    public void KillPlayer(float FirstTimer = 0, float SecondTimer = 1) {
        if(!isRunning) {
            isRunning = true;
            Player.GetComponent<CharacterController>().canMove = false;
            StartCoroutine(DeathRespawn(FirstTimer, SecondTimer));
        }
    }

    IEnumerator DeathRespawn(float FirstTimer, float SecondTimer)
    {
        yield return new WaitForSeconds(FirstTimer);
        MainCam.transform.parent.GetComponent<CameraController>().enabled = false;
        Player.SetActive(false);
        yield return new WaitForSeconds(SecondTimer);
        MainCam.transform.parent.GetComponent<CameraController>().enabled = true;
        Spawn();
        Player.GetComponent<CharacterController>().SetState(0);
        MainCam.transform.parent.GetComponent<CameraController>().ChangeDir();
        Player.GetComponent<CharacterController>().canMove = true;

        isRunning = false;
    }

    //The Player is Respawned
    private void Spawn()
    {
        if (SpawnPoint == null && GameObject.FindGameObjectWithTag("SpawnPoint"))
            SpawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");
        else if (SpawnPoint == null)
            return;

        Player.transform.position = SpawnPoint.transform.position;
        Player.SetActive(true);
    }

    //Player Loses a Life
    private void LoseLife() {


    }

    public void LoseSinglePlayer() {
        //Takes the player to the Leader Boards
        SceneManager.LoadScene("LeaderBoard");
        Started = false;
    }
}
