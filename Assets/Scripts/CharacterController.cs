using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float doubleJumpForce =5;
    [SerializeField] private float rayCastLength;
    //private enum monsterSide {front, left, right, back} //states for decided which direction player is moving based on which side on the monster
    public int state = 0;

    Rigidbody rb;
    private bool canMove = true;

    //Directions
    private bool canDoubleJump = true;
    private bool topSection = true;
    public GameObject[] TopBounds;
    private bool isRunning = false;

    public LayerMask layerMask;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }



    void FixedUpdate()
    {
        if (canMove) {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement(horizontal, vertical);
        }
    }


    private void Movement(float hor, float ver){
        Vector3 newVel = Direction() * playerSpeed * hor;
        rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);

        Jump();

        CheckSwitch(hor);
    }

    private void Jump() {
        //Jump
        print(CheckGrounded());
        if (Input.GetKeyDown("space") && !CheckGrounded() && canDoubleJump)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            canDoubleJump = false;
        }
        if (Input.GetKeyDown("space") && CheckGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            canDoubleJump = true;
        }
    }

    private bool CheckGrounded() {
        // use for seeing raycast Debug.DrawRay(physicsCenter, Vector3.down * rayCastLength, Color.red);
        Vector3 physicsCenter = transform.position + GetComponent<BoxCollider>().center;
        Debug.DrawRay(physicsCenter, Vector3.down * rayCastLength, Color.red);
        return Physics.Raycast(physicsCenter, Vector3.down, rayCastLength, layerMask);
    }

    void CheckSwitch(float hor){
        if (!CheckGrounded())
            return;
        switch (state)
        {
            case 0: //Front
                if (transform.position.x > TopBounds[1].transform.position.x && hor > 0)
                    state = 1;
                if (transform.position.x < TopBounds[0].transform.position.x && hor < 0)
                    state = 3;
                break;
            case 1: //Right
                if (transform.position.z > TopBounds[2].transform.position.z && hor > 0)
                    state = 2;
                if (transform.position.z < TopBounds[1].transform.position.z && hor < 0)
                    state = 0;
                break;
            case 2: //Back
                if (transform.position.x < TopBounds[3].transform.position.x && hor > 0)
                    state = 3;
                if (transform.position.x > TopBounds[2].transform.position.x && hor < 0)
                    state = 1;
                break;
            case 3: //Left
                if (transform.position.z < TopBounds[0].transform.position.z && hor > 0)
                    state = 0;
                if (transform.position.z > TopBounds[3].transform.position.z && hor < 0)
                    state = 2;
                break;
        }
    }

    //states for checking direction
    private Vector3 Direction(){
        switch (state){
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
}
