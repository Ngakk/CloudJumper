using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Velocity = 10;

    private Rigidbody rigi;

    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();

    }

    private void Move()
    {
        float screenPointX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        float dir = screenPointX - transform.position.x;

        //transform.position += Vector3.right * dir * Time.deltaTime * Velocity;

        rigi.velocity = new Vector3(dir * Velocity, rigi.velocity.y, rigi.velocity.z);
    }
}
