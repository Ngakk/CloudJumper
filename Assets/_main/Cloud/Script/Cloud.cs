using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.SendMessageUpwards("Jump", SendMessageOptions.DontRequireReceiver);
        SelfDestroy();
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
