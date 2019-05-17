using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    public Movement player;

    private float StartingPos = 0.0f;
    private float MaxHeight = 0.0f;
    private bool SavedStats = false;

    private void Awake()
    {
        StaticManager.cloudSpawner = this;
    }

    private void Start()
    {
        StartingPos = player.transform.position.y;
    }

    private void Update()
    {
        if(MaxHeight < player.transform.position.y)
        {
            MaxHeight = player.transform.position.y;
        }
    }

    public void SaveStats(int cloudsTouched)
    {
        //StaticManager.netUtilities.UpdateScore(PlayerPrefs.GetString("username"), player.CloudsTouched, (int)(MaxHeight-StartingPos));
        Debug.Log("Savestats");
    }
}
