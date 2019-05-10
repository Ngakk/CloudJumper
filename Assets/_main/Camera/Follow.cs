using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform stalked;

    private void Update()
    {
        FollowY();
    }

    private void FollowY()
    {
        transform.Translate(Vector3.Scale((stalked.position - transform.position), Vector3.up));
    }
}
