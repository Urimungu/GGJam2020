using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class EnemyMovement : MonoBehaviour
{
    //Player Stats
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float gravity = 0.7f;
    [SerializeField] private float rayCastLength = 0.4f;

    //References
    private Rigidbody rb;
    public bool canMove = true;
    public int State;
    public bool SpaceBar;

    public List<Vector3> Bounds = new List<Vector3>();
    private Animator anim;
    public LayerMask layerMask;

    //Control
    public float Horizontal;

    void Start() {
        StartEnemy();

    }

    public void ControlTheEnemy(float hor, bool Jump = false) {
        Horizontal = hor;
        SpaceBar = Jump;
    }

    //Initializes the Enemy
    public void StartEnemy() {
        if (rb == null || anim == null) {
            rb = GetComponent<Rigidbody>();
            anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }
        //Updates the bounds to match the Player's
        Bounds = GameManager.Manager.Player.GetComponent<CharacterController>().Bounds;
    }

    private void Update() {
        //If the player is unable to move, don't let him move
        if(canMove)
            Movement(Horizontal);

        //Looks the Direction it is Moving
        if(Mathf.Abs(new Vector2(rb.velocity.x, rb.velocity.z).magnitude) > 0.1f)
            transform.GetChild(0).rotation =
            Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z), Vector3.up);
    }

    //Moves the Player
    private void Movement(float hor) {
        //Moves the player in the Direction that he is compared to the Robot
        Vector3 newVel = Direction() * playerSpeed * hor;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y - gravity, newVel.z);

        //Lets the player jump
        Jump();

        //Switches the player if he steps past the bounds
        CheckSwitch(hor);
    }

    //Jumping Mechanics
    private void Jump() {
        //Initial Jump Condition
        if(SpaceBar && CheckGrounded()) {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            SpaceBar = false;
        }
    }

    //Checks Ground Collision
    public bool CheckGrounded() {
        Vector3 physicsCenter = transform.position + GetComponent<CapsuleCollider>().center;
        return Physics.Raycast(physicsCenter - new Vector3(0 , (GetComponent<CapsuleCollider>().height / 2) - 0.1f,0), Vector3.down, rayCastLength, layerMask);
    }

    //Turns the player to the direction that he is on the robot
    void CheckSwitch(float hor) {
        //Don't switch if the player isn't on a plate form
        if(!CheckGrounded())
            return;

        //Checks to see what direction the player is on
        switch(State) {
            case 0: //Front
            if(transform.position.x > Bounds[1].x && hor > 0)
                State = 1;
            if(transform.position.x < Bounds[0].x && hor < 0)
                State = 3;
            break;
            case 1: //Right
            if(transform.position.z > Bounds[2].z && hor > 0)
                State = 2;
            if(transform.position.z < Bounds[1].z && hor < 0)
                State = 0;
            break;
            case 2: //Back
            if(transform.position.x < Bounds[3].x && hor > 0)
                State = 3;
            if(transform.position.x > Bounds[2].x && hor < 0)
                State = 1;
            break;
            case 3: //Left
            if(transform.position.z < Bounds[0].z && hor > 0)
                State = 0;
            if(transform.position.z > Bounds[3].z && hor < 0)
                State = 2;
            break;
        }
    }

    //states for checking direction
    public Vector3 Direction() {
        switch(State) {
            case 0: //Front
            return Vector3.right;
            case 1: //Right
            return Vector3.forward;
            case 2: //Back
            return Vector3.left;
            case 3: //Left
            return Vector3.back;
        }
        return Vector3.zero;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("DeathZone"))
            Destroy(gameObject);
    }
}
