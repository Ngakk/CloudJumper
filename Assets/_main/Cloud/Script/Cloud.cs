using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    private void Update() {
        if(StaticManager.cloudSpawner.player.transform.position.y - 8 > transform.position.y){
            SelfDestroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Col");
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
