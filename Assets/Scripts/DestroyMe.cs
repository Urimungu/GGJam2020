using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMe : MonoBehaviour
{
    [SerializeField] private float timer = 1f;
    private float createTime = 0.0f;

    private void Start()
    {
        createTime = Time.time;
    }
    private void Update()
    {

        if ((Time.time - createTime) > timer)
        {
            Destroy(gameObject);
        }
    }


}
