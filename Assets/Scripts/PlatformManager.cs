using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<GameObject> UpPlatforms =  new List<GameObject>();
    public List<GameObject> RightPlatforms = new List<GameObject>();

    public float speed = 4.0f;

    bool movingRight = true;
    bool movingUp = true;

    public Vector3 right = new Vector3(), left = new Vector3(), up = new Vector3(), down = new Vector3();

    public float distance = 2.0f;

    private void Start()
    {
        right = transform.position + Vector3.right * distance;
        left = transform.position - (Vector3.right * distance);
        up = transform.position + Vector3.up * distance;
        down = transform.position - (Vector3.up * distance);
    }

    void Update()
    {
        if (movingRight)
        {
            transform.position = Vector3.Slerp(transform.position, right, Time.deltaTime * speed);
        }
        else if(!movingRight)
        {
            transform.position = Vector3.Slerp(transform.position, left, Time.deltaTime * speed);
        }

         
        if(movingUp)
        {
            transform.position = Vector3.Slerp(transform.position, up, Time.deltaTime * speed);
        }
        else if(!movingUp)
        {
            transform.position = Vector3.Slerp(transform.position, down, Time.deltaTime * speed);
        }
    }
}