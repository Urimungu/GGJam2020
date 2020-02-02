using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<GameObject> platforms =  new List<GameObject>();
    private string side;
    private string color;

    public float speed = 4.0f;
    bool movingRight;
    public Vector3 right = new Vector3(), left = new Vector3(), up = new Vector3(), down = new Vector3();
    public float distance;

    private void Start()
    {
        right = transform.position + Vector3.right * distance;
        left = transform.position - (Vector3.right * distance);
        up = transform.position + Vector3.up * distance;
        down = transform.position - (Vector3.up * distance);
    }

    void Update()
    {
        if (movingRight) {
            transform.position = Vector3.Slerp(transform.position, right, 0.3f);
        }
    }

    //Right
        //both 2 left 2 right

    //Back
        //Far Right/left 2 both blue
        //Middle Right/left 2.5 both blue

    //Left
        //BlueRight up 5 down 1.5
        //green right 3.5 both
        //green left 3.5 both
        //BlueLeft down 5

    //Front
    //BlueRight 2.5 both
    //GreenLeft 2 both
    //greenright 2 both
}
