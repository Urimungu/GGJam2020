using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Singleton
    public static GameManager Manager;

    [Header("Camera")][SerializeField]
    public Camera MainCam;

    public GameObject Player;

    private void Awake(){
        if (Manager == null) {
            Manager = GetComponent<GameManager>();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetCamera(Camera cam) {
        MainCam = cam;
    }

    public void StartGame()
    {


    }

}
