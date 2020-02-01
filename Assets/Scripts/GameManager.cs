using System.Collections;
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
        MainCam = GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject;
        MainCam.transform.GetComponent<CameraController>().Player = Player.transform;
        MainCam.transform.GetComponent<CameraController>().enabled = true;
        MainCam.transform.GetComponent<CameraController>().ChangeDir();
    }

    public void KillPlayer() {
        MainCam.GetComponent<CameraController>().enabled = false;
        Player.SetActive(false);
        Player.transform.position = SpawnPoint.transform.position;
        if(!isRunning) {
            isRunning = true;
            StartCoroutine(DeathRespawn());
        }
    }

    IEnumerator DeathRespawn()
    {
        yield return new WaitForSeconds(1);
        MainCam.GetComponent<CameraController>().enabled = true;
        Player.SetActive(true);
        Player.GetComponent<CharacterController>().SetState(0);
        MainCam.GetComponent<CameraController>().ChangeDir();

        isRunning = false;
    }

    //The Player is Respawned
    private void Spawn() {
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
