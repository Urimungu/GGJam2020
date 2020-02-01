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
    public Camera MainCam;
    public GameObject Player;

    //Variables
    private bool Started = false;
    private bool SinglePlayer = true;

    //Navigation
    public Transform TopDoor, BottomDoor;
    public GameObject[] TopBounds = new GameObject[4];
    public GameObject[] BottomBounds = new GameObject[4];

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
            GatherNavigation();
        }

    }

    private void GatherNavigation() {
        //Sets size of Arrays
        TopBounds = new GameObject[4];
        BottomBounds = new GameObject[4];
        //Finds the Bounds in the Scene
        Transform temp = GameObject.FindGameObjectWithTag("Bounds").transform;

        //Fills in the Arrays
        for (int i = 0; i < 4; i++) {
            TopBounds[i] = temp.GetChild(0).GetChild(i).gameObject;
            BottomBounds[i] = temp.GetChild(1).GetChild(i).gameObject;
        }

        TopDoor = temp.GetChild(2).GetChild(0);
        BottomDoor = temp.GetChild(2).GetChild(1);
    }

    public void LoseSinglePlayer() {
        //Takes the player to the Leader Boards
        SceneManager.LoadScene("LeaderBoard");
        Started = false;
    }
}
