using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Player Stats
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float doubleJumpForce =5;
    [SerializeField] private float rayCastLength;

    //References
    private Rigidbody rb;
    private bool canMove = true;
    private int State;

    //Directions
    private bool canDoubleJump = true;
    private bool topSection = true;
    public GameObject[] TopBounds;
    private bool isRunning = false;

    public LayerMask layerMask;

    //Initializes the Player
    void Start(){
        rb = GetComponent<Rigidbody>();
        GameManager.Manager.Player = gameObject;
    }


    void FixedUpdate() {
        //If the player is unable to move, don't let him move
        if (canMove) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }
    }

    //Moves the Player
    private void Movement(float hor, float ver){
        //Moves the player in the Direction that he is compared to the Robot
        Vector3 newVel = Direction() * playerSpeed * hor;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);

        Jump();

        //Switches the player if he steps past the bounds
        CheckSwitch(hor);
    }

    //Jumping Mechanics
    private void Jump() {
        //Double Jumps if the player is able to
        print(CheckGrounded());
        if (Input.GetKeyDown("space") && !CheckGrounded() && canDoubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            canDoubleJump = false;
        }
        //Initial Jump Condition
        if (Input.GetKeyDown("space") && CheckGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            canDoubleJump = true;
        }
    }

    //Checks Ground Collision
    private bool CheckGrounded() {
        Vector3 physicsCenter = transform.position + GetComponent<BoxCollider>().center;
        return Physics.Raycast(physicsCenter, Vector3.down, rayCastLength, layerMask);
    }

    //Turns the player to the direction that he is on the robot
    void CheckSwitch(float hor){
        //Don't switch if the player isn't on a plateform
        if (!CheckGrounded())
            return;
        //Checks to see what direction the player is on
        switch (State) {
            case 0: //Front
                if (transform.position.x > TopBounds[1].transform.position.x && hor > 0)
                    State = 1;
                if (transform.position.x < TopBounds[0].transform.position.x && hor < 0)
                    State = 3;
                break;
            case 1: //Right
                if (transform.position.z > TopBounds[2].transform.position.z && hor > 0)
                    State = 2;
                if (transform.position.z < TopBounds[1].transform.position.z && hor < 0)
                    State = 0;
                break;
            case 2: //Back
                if (transform.position.x < TopBounds[3].transform.position.x && hor > 0)
                    State = 3;
                if (transform.position.x > TopBounds[2].transform.position.x && hor < 0)
                    State = 1;
                break;
            case 3: //Left
                if (transform.position.z < TopBounds[0].transform.position.z && hor > 0)
                    State = 0;
                if (transform.position.z > TopBounds[3].transform.position.z && hor < 0)
                    State = 2;
                break;
        }
    }

    //states for checking direction
    private Vector3 Direction(){
        switch (State) {
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

    //Returns the Current state of the player
    public int GetState() {
        return State;
    }
}
