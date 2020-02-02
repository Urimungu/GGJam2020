using UnityEngine;

public class RotateZForever : MonoBehaviour
{
    [SerializeField] private int speed = 50;
    
    void FixedUpdate()
    {
        transform.Rotate (0,0, speed * Time.deltaTime);
    }
}
