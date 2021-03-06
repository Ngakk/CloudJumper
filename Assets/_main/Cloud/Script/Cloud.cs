﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    public Movement playerToFollow;

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
