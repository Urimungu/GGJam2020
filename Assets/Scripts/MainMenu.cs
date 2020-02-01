using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //References
    public Text Options;
    
    //Variables
    private int State = 0;
    private bool canMove = true;

    void Update()
    {
        //Controls the States
        if (canMove) {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                State = BoundsTracker(false);
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                State = BoundsTracker(true);
        }
        //Runs the Display
        Display();
    }

    //Makes sure the state doesn't fall out of bounds
    private int BoundsTracker(bool add) {
        int newState = State + (add ? 1 : -1);
        var adjustedNewState = newState > 3 ? 0 : newState < 0 ? 3 : newState;
        if (adjustedNewState != State)
            Message.Publish(new UiSelectionChanged());
        return adjustedNewState;
    }

    //Updates the Text on Screen for a Main Menu Feel
    void Display() {
        switch(State) {
            //Start
            case 0:
                Options.text = ">>Start<<\n2-Player Start\nLeader Board\nQuit\n\n";
                LoadGame();
                break;
            //2 Player
            case 1:
                Options.text = "Start\n>>2-Player Start<<\nLeader Board\nQuit\n\n";
                LoadTwoPlayer();
            break;
            //Leader Board
            case 2:
                Options.text = "Start\n2-Player Start\n>>Leader Board<<\nQuit\n\n";
                LeaderBoard();
            break;
            //Quit
            case 3:
                //Displays the Text for Quit
                Options.text = "Start\n2-Player Start\nLeader Board\n>>Quit<<\n\n";
                Quit();
                break;
        }
    }

    //Loads the Game
    private void LoadGame() {
        if(Input.GetKeyDown(KeyCode.Return))
            LoadScene("SinglePlayer");
    }

    //Loads the 2 player game
    private void LoadTwoPlayer()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            LoadScene("TwoPlayer");
    }

    //Takes the Player to the LeaderBoard Screen
    private void LeaderBoard() {
        if (Input.GetKeyDown(KeyCode.Return))
            LoadScene("LeaderBoard");
    }

    private void LoadScene(string sceneName)
    {
        Message.Publish(new UiConfirmed());
        SceneManager.LoadScene(sceneName);
    }

    //Quits if the person decides to Exit the game
    private void Quit() {
        //If the Press enter it takes them to a 'Are you Sure' Screen
        if(Input.GetKeyDown(KeyCode.Return) || !canMove) {
            Options.text = "Quit?\n[Y]es/[N]o\n\n\n";
            canMove = false;
            //Press Y to Quit and N to return to menu
            if(Input.GetKeyDown(KeyCode.Y))
                Application.Quit();
            if(Input.GetKeyDown(KeyCode.N))
                canMove = true;
        }
    }
}
