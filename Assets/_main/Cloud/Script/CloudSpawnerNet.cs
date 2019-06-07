﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CloudSpawnerNet : NetworkBehaviour
{
    public Movement player;
    public Movement player2;
    public GameEndUI ui;
    public GameObject cloudPrefab;
    public GameObject cloudPrefab2;
    private float StartingPos = 0.0f;
    private float MaxHeight = 0.0f;
    private float MaxHeight2 = 0.0f;
    float MaxHeight3 = 2.0f;
    private float NextCloudDistance = 2.0f;
    private bool SavedStats = false;

    private void Awake()
    {
        StaticManager.cloudSpawnerNet = this;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(MaxHeight < player.transform.position.y)
        {
            MaxHeight = player.transform.position.y;
        }
        
        if(MaxHeight2 < player.transform.position.y)
        {
            SpawnClouds();
        }
    }

    public void StartGame()
    {
        StartingPos = player.transform.position.y;
        ui.gameObject.SetActive(false);

        MaxHeight2 = -4;
        SpawnClouds();
    }

    public void SpawnClouds()
    {    
        float t = (MaxHeight2 > 100 ? 100 : MaxHeight2)/100.0f;
        if(t < 0)
            t = 0;

        float downset = Mathf.Lerp(t, -4.0f, 0.0f);
        float upset = Mathf.Lerp(t, 4.0f, 8.0f);

        float height_many = 100 - (MaxHeight2 > 100 ? 100 : MaxHeight2);

        if(height_many< 0)
            height_many = 0;

        int how_many = Mathf.FloorToInt(height_many / 25.0f) + 1;

        for(int i = 0; i < how_many; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-4.0f, 4.0f), MaxHeight2 + 8 + Random.Range(downset, upset), 0);
            GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity);

            NetworkServer.Spawn(go);
        }

        MaxHeight2 += 4;
    }

    public void SaveStats(int cloudsTouched)
    {
        ui.gameObject.SetActive(true);
        ui.SetTexts(player.CloudsTouched.ToString(), ((int)(MaxHeight-StartingPos)).ToString());
        StaticManager.netUtilities.UpdateScore(PlayerPrefs.GetString("username"), player.CloudsTouched, (int)(MaxHeight-StartingPos));
        Debug.Log("Savestats");
        
    }

    public void ToMainMenu(){
        SceneManager.LoadScene(0);
    }
}
