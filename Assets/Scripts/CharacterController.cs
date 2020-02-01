using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Player Stats
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float doubleJumpForce =5;
    [SerializeField] private float gravity = 1;
    [SerializeField] private float rayCastLength;

    //References
    private Rigidbody rb;
    private bool canMove = true;
    private int State ;         //Needs to Start at -1 so that it sets the bounds in game
    public List<GameObject> Bounds = new List<GameObject>();
    public ParticleSystem rocketFire;

    //Variables
    private bool canDoubleJump = true;
    private bool topSection = true;
    private bool isRunning = false;
    private bool inRange;
    private bool onBottom;

    public LayerMask layerMask;

    //Initializes the Player
    void Start(){
        rb = GetComponent<Rigidbody>();
        GameManager.Manager.Player = gameObject;
        rocketFire = GetComponent<ParticleSystem>();
    }


    private void Update() {
        //If the player is unable to move, don't let him move
        if (canMove) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }
        //When the player gets in range and they press up they Travel across the map
        if(inRange && Input.GetKeyDown(KeyCode.W))
            SwitchDoor();
    }

    //Moves the Player
    private void Movement(float hor, float ver){
        //Moves the player in the Direction that he is compared to the Robot
        Vector3 newVel = Direction() * playerSpeed * hor;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y -gravity , newVel.z);

        //Lets the player jump
        Jump();

        //Switches the player if he steps past the bounds
        CheckSwitch(hor);
    }

    //Jumping Mechanics
    private void Jump() {
        //Double Jumps if the player is able to
        if (Input.GetKeyDown("space") && canDoubleJump && !CheckGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, doubleJumpForce, rb.velocity.z);
            rocketFire.Play();
            canDoubleJump = false;
        }
        //Initial Jump Condition
        if (Input.GetKeyDown("space") && CheckGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            canDoubleJump = true;
            rocketFire.Stop();
        }
    }

    //Checks Ground Collision
    private bool CheckGrounded() {
        Vector3 physicsCenter = transform.position + GetComponent<BoxCollider>().center;
        return Physics.Raycast(physicsCenter, Vector3.down, rayCastLength, layerMask);
    }

    //Turns the player to the direction that he is on the robot
    void CheckSwitch(float hor){
        //Don't switch if the player isn't on a plate form
        if (!CheckGrounded() || Bounds.Count == 0) {
            if(Bounds.Count == 0)
            for (int i = 0; i < 4; i++)
                {
                    GameObject SomethingIdk = new GameObject();
                    SomethingIdk.transform.position = GameManager.Manager.TopBounds[i].transform.position;
                    Bounds.Add(SomethingIdk);
                }
            return;
        }
        //Checks to see what direction the player is on
        switch (State) {
            case 0: //Front
                if (transform.position.x > Bounds[1].transform.position.x && hor > 0)
                    State = 1;
                if (transform.position.x < Bounds[0].transform.position.x && hor < 0)
                    State = 3;
                break;
            case 1: //Right
                if (transform.position.z > Bounds[2].transform.position.z && hor > 0)
                    State = 2;
                if (transform.position.z < Bounds[1].transform.position.z && hor < 0)
                    State = 0;
                break;
            case 2: //Back
                if (transform.position.x < Bounds[3].transform.position.x && hor > 0)
                    State = 3;
                if (transform.position.x > Bounds[2].transform.position.x && hor < 0)
                    State = 1;
                break;
            case 3: //Left
                if (transform.position.z < Bounds[0].transform.position.z && hor > 0)
                    State = 0;
                if (transform.position.z > Bounds[3].transform.position.z && hor < 0)
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

    private void SwitchDoor() {
        //Goes to the Bottom
        if(!onBottom) {
            transform.position = GameManager.Manager.BottomDoor.position;
            Bounds.Clear();
            for(int i = 0; i < 4; i++){
                Bounds[i].transform.position = GameManager.Manager.BottomBounds[i].transform.position;
            }
            onBottom = true;
            print("Made into Bottom");
        }else {
            //Goes to the Top
            transform.position = GameManager.Manager.TopDoor.position;
            Bounds.Clear();
            for (int i = 0; i < 4; i++) {
                Bounds[i].transform.position = GameManager.Manager.TopBounds[i].transform.position;
            }
            onBottom = false;
            print("Made into Top");

        }
    }

    private void OnTriggerEnter(Collider other) {
        //Lets you press the button to move up or down
        if (other.name == "TopDoor" || other.name == "BottomDoor")
            inRange = true;
    }

    private void OnTriggerExit(Collider other) {
        //Lets you press the button to move up or down
        if(other.name == "TopDoor" || other.name == "BottomDoor")
            inRange = false;
    }

    //Returns the Current state of the player
    public int GetState() {
        return State;
    }
}
