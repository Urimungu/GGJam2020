using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightPlats : MonoBehaviour
{
    bool movingRight = true;

    public Vector3 right = new Vector3(), left = new Vector3();
    public float distance = 2.0f;

    public float speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        right = transform.position + Vector3.right * distance;
        left = transform.position - (Vector3.right * distance);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingRight)
        {
            transform.position = Vector3.Slerp(transform.position, right, Time.deltaTime * speed);
        }
        else if (!movingRight)
        {
            transform.position = Vector3.Slerp(transform.position, left, Time.deltaTime * speed);
        }
    }
}
