using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MovementNet : NetworkBehaviour
{
    public GameObject mainModel, transparentModel;
    public float Velocity = 5;
    public float JumpForce = 10;
    public float speedFalloff = 0.3f;
    public int playerId;
    private int connectionId;
    public MovementNet otherPlayer;
    private bool otherLost = false, iLost = false;
    bool bandera;

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

            Camera.main.GetComponent<Follow>().stalked = transform;
            
            StaticManager.localPlayer = this;

            SceneManager.sceneLoaded += OnLevelLoad;
        }

        if(!isServer)
            Cmd_SendSearchPlayers();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoad;
    }



    private void OnLevelLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 2)
        {
            Invoke("Initialize", 0.3f);
            Invoke("CallStartGame", 0.6f);
        }
    }

    private void Initialize()
    {
        StaticManager.cloudSpawnerNet.player = this;
    }

    private void CallStartGame()
    {
        StaticManager.cloudSpawnerNet.StartGame();
    }


    [Command]
    public void Cmd_SendSearchPlayers()
    {
        Rpc_SearchPlayers();
    }

    [ClientRpc]
    void Rpc_SearchPlayers()
    {
        SearchPlayers();
        //Invoke("SearchPlayers", 1f);
    }

    void SearchPlayers()
    {

        if(otherPlayer != null)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        //Debug.Log("Found " + players.Length + " player(s)");

        bool bandera = true;
        for (int i = 0; i < players.Length; i++)
        {
            MovementNet movenet = players[i].GetComponent<MovementNet>();
            if(movenet != null)
            {
                print(!movenet.isLocalPlayer);
                print(movenet != this);
                if (movenet != this)
                {
                    otherPlayer = movenet;
                    otherPlayer.otherPlayer = this;
                    Debug.Log("Found the other player");
                    bandera = false;
                    break;
                }
            }
        }
        if(bandera)
            Debug.Log("Didn't Found the other player");
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
    
    void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space) && otherPlayer == null)
        {
            SearchPlayers();
        }*/

        if (isLocalPlayer)
        {
            Move();

            if (Input.GetKeyDown(KeyCode.Space) && !UsedJump && canJump && otherPlayer != null)
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
    
    public void OtherPlayerLost()
    {
        otherLost = true;
        if(iLost)
        {
            StaticManager.cloudSpawnerNet.SaveStats(CloudsTouched);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Finish") && UsedJump)
        {
            otherPlayer.OtherPlayerLost();
            iLost = true;
            if (otherLost)
            {
                StaticManager.cloudSpawnerNet.SaveStats(CloudsTouched);
            }
            else
            {
                if(isLocalPlayer)
                {
                    Camera.main.GetComponent<Follow>().stalked = otherPlayer.transform;
                }
            }
        }
    }
}
