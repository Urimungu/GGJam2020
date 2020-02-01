using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class CityMover : MonoBehaviour
{
    public float Speed = 1;
    public List<GameObject> Parts = new List<GameObject>();
    private float moveZ;

    void FixedUpdate()
    {
        moveZ += Speed;
        transform.position = new Vector3(0, -50, moveZ);
        if (moveZ >= 100) {
            moveZ = 0;
            Parts.Add(Parts[0]);
            Parts.Remove(Parts[0]);
            transform.position = new Vector3(0, -50, 0);
            for(int i = 0; i < 4; i++)
                Parts[i].transform.position = new Vector3(0, -50, 100 - (100 * i));
        }
    }
}
