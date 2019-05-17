using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Velocity = 10;
    public float JumpForce = 10;

    private Rigidbody rigi;

    public bool UsedJump = false;
    public int CloudsTouched = -1;

    private void Start()
    {
        rigi = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.Space) && !UsedJump)
        {
            Jump();
            UsedJump = true;
        }
    }

    private void Move()
    {
        float screenPointX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        float dir = screenPointX - transform.position.x;

        //transform.position += Vector3.right * dir * Time.deltaTime * Velocity;

        rigi.velocity = new Vector3(dir * Velocity, rigi.velocity.y, rigi.velocity.z);
    }

    private void Jump()
    {
        Debug.Log("jupmp");
        
        if(rigi.velocity.y < 0)
        {
            rigi.velocity = new Vector3(rigi.velocity.x, 0, rigi.velocity.z);
        }

        rigi.AddForce(Vector3.up * JumpForce);

        CloudsTouched++;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Finish") && UsedJump)
        {
            StaticManager.cloudSpawner.SaveStats(CloudsTouched);
        }
    }
}
