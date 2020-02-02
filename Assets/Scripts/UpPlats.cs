using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpPlats : MonoBehaviour
{
    public Vector3 up = new Vector3(), down = new Vector3();

    public float distance = 2.0f;
    bool movingUp = true;

    public float speed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        up = transform.position + Vector3.up * distance;
        down = transform.position - (Vector3.up * distance);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingUp)
        {
            transform.position = Vector3.Slerp(transform.position, up, Time.deltaTime * speed);
        }
        else if (!movingUp)
        {
            transform.position = Vector3.Slerp(transform.position, down, Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GoDown"))
            movingUp = false;
        else if (other.CompareTag("GoUp"))
            movingUp = true;
    }
}
