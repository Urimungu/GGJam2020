﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform Player;
    private bool CanMove = true;
    public Vector2 offset;      //(offset, height)
    public Vector3 Offset;
    private int oldInt;

    void Start() {
        //Makes sure the Player is not set to null
        if (GameManager.Manager != null && GameManager.Manager.Player != null)
            Player = GameManager.Manager.Player.transform;
        ChangeDir();
    }

    void FixedUpdate() {
        if (CanMove)
            Movement();

        //Checks to make to see if the player changed states
        if (Player.GetComponent<CharacterController>().state == oldInt) return;
        ChangeDir();
        oldInt = Player.GetComponent<CharacterController>().state;
    }

    void Movement() {
        //Slerps to the new Camera position
        transform.position = Vector3.Slerp(transform.position, Player.position + Offset, 0.2f);
        //Looks at the player
        transform.LookAt(Player.position, Vector3.up);
    }

    //Changes the Camera Offset if the Player Offset Changes
    private void ChangeDir() {
        switch (Player.GetComponent<CharacterController>().state) {
            case 0: //Front
                Offset = new Vector3(0, offset.y, -offset.x);
                break;
            case 1: //Right
                Offset = new Vector3(offset.x, offset.y, 0);
                break;
            case 2: //Back
                Offset = new Vector3(0, offset.y, offset.x);
                break;
            case 3: //Left
                Offset = new Vector3(-offset.x, offset.y, 0);
                break;
        }
    }
}
