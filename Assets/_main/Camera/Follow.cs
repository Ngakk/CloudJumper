using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform stalked;

    public float YOffset = 40;

    private void Update()
    {
        if(stalked != null)
            FollowY();
    }

    private void FollowY()
    {
        Vector3 stalkedY = Vector3.Scale(stalked.position, Vector3.up) + Vector3.up * YOffset;
        Vector3 ownY = Vector3.Scale(transform.position, Vector3.up);

        transform.Translate((stalkedY - ownY));
    }
}
