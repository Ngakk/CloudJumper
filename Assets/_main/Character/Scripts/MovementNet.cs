using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MovementNet : NetworkBehaviour
{
    public GameObject mainModel, transparentModel;
    public float Velocity = 5;
    public float JumpForce = 10;
    public float speedFalloff = 0.3f;
    public int playerId;
    private int connectionId;

    private Rigidbody rigi;

    [HideInInspector]
    public bool UsedJump = false;
    [HideInInspector]
    public bool canJump = true;
    [HideInInspector]
    public int CloudsTouched = -1;

    private void Start()
    {
        rigi = GetComponent<Rigidbody>();

        if (isLocalPlayer)
        {
            print("is local");
            mainModel.SetActive(true);
            transparentModel.SetActive(false);

            gameObject.layer = 9;
            ChangeChildLayers(transform, 9);
        }
    }

    private void ChangeChildLayers(Transform _parent, int _layer)
    {
        for(int i = 0; i < _parent.childCount; i++)
        {
            Transform child = _parent.GetChild(i);
            child.gameObject.layer = _layer;
            ChangeChildLayers(child, _layer);
        }
    }

    public override void OnStartClient()
    {
        /*if (isLocalPlayer)
        {
            print("is local");
            mainModel.SetActive(true);
            transparentModel.SetActive(false);
            gameObject.layer = 9;
        }
        else
        {
            print("is not local");
            mainModel.SetActive(false);
            transparentModel.SetActive(true);
            gameObject.layer = 10;
        }*/
    }

    public override void OnStartLocalPlayer()
    {
        /*print("is local");
        mainModel.SetActive(true);
        transparentModel.SetActive(false);
        gameObject.layer = 9;*/
    }


    void Update()
    {
        if (isLocalPlayer) //TODO: cambiar de localPlayer a connection number o algo
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space) && !UsedJump && canJump)
            {
                Jump();
                UsedJump = true;
            }
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
        
        if(rigi.velocity.y < 0)
        {
            rigi.velocity = new Vector3(rigi.velocity.x, 0, rigi.velocity.z);
        }
        else{
            rigi.velocity = new Vector3(rigi.velocity.x, rigi.velocity.y*speedFalloff, rigi.velocity.z);
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
