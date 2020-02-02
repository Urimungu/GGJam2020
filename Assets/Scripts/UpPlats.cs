using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPlats : MonoBehaviour
{
    public Vector3 up = new Vector3(), down = new Vector3();

    public float distance = 2.0f;
    bool movingUp = true;

    private float speed = 2.0f;

    private float yLoc;

    void Awake()
    {
        yLoc = transform.position.y;
        up = transform.position + Vector3.up * distance;
        down = transform.position - (Vector3.up * distance);
    }

    void Update()
    {
        if (movingUp)
        {
            transform.position = Vector3.Lerp(transform.position, up, Time.deltaTime * speed);
        }
        else if (!movingUp)
        {
            transform.position = Vector3.Lerp(transform.position, down, Time.deltaTime * speed);
        }

        if (transform.position.y >= yLoc + 1.9f)
            movingUp = false;
        if (transform.position.y <= yLoc - 1.9f)
            movingUp = true;

       // print(transform.position.y);
        //print(yLoc);
    }
}
