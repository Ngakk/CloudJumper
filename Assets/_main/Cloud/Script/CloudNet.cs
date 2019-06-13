using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CloudNet : NetworkBehaviour
{

    public MovementNet playerToFollow;

    private void Update() {
        if(playerToFollow.transform.position.y - 8 > transform.position.y){
            SelfDestroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.SendMessageUpwards("Jump", SendMessageOptions.DontRequireReceiver);
            SelfDestroy();
        }
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
