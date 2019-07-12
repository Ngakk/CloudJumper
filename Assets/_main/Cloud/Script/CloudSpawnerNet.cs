using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CloudSpawnerNet : NetworkBehaviour
{
    public static int playerConId = 0;

    public int myId = 0;
    public MovementNet player;
    public GameEndUI ui;
    public GameObject cloudPrefab;
    private float StartingPos = 0.0f;
    private float MaxHeight = 0.0f;
    private float MaxHeight2 = 0.0f;
    private float MaxHeight3 = 0.0f;
    private float MaxHeight4 = 0.0f;
    private float NextCloudDistance = 2.0f;
    private bool SavedStats = false;

    private void Awake()
    {
        StaticManager.cloudSpawnerNet = this;
    }

    private void Start()
    {
        ui.gameObject.SetActive(false);
        playerConId = myId;
    }

    public void StartGame()
    {
        StartingPos = player.transform.position.y;
        MaxHeight2 = -8;
        MaxHeight4 = -8;
        Cmd_SpawnClouds(true);
    }

    private void Update()
    {
        if (hasAuthority)
        {
            if (player != null)
            {
                if (MaxHeight < player.transform.position.y)
                {
                    MaxHeight = player.transform.position.y;
                }

                if (MaxHeight2 <player.transform.position.y)
                {
                    Cmd_SpawnClouds(true);
                }



                if (player.otherPlayer != null)
                {
                    if (MaxHeight3 < player.otherPlayer.transform.position.y)
                    {
                        MaxHeight3 = player.otherPlayer.transform.position.y;
                    }

                    if (MaxHeight4 < player.otherPlayer.transform.position.y)
                    {
                        Cmd_SpawnClouds(false);
                    }
                }
            }

            
        }
    }

    [Command]
    public void Cmd_SpawnClouds(bool _b)
    {
        Debug.Log("Spawning clouds");
        float t_height = _b ? MaxHeight2 : MaxHeight4;

        float t = (t_height > 100 ? 100 : t_height) / 100.0f;
        if (t < 0)
            t = 0;

        float downset = Mathf.Lerp(t, -4.0f, 0.0f);
        float upset = Mathf.Lerp(t, 4.0f, 8.0f);

        float height_many = 100 - (t_height > 100 ? 100 : t_height);

        if (height_many < 0)
            height_many = 0;

        int how_many = Mathf.FloorToInt(height_many / 25.0f) + 1;

        for (int i = 0; i < how_many; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-4.0f, 4.0f), t_height + 8 + Random.Range(downset, upset), 0);
            GameObject go = Instantiate(cloudPrefab, pos, Quaternion.identity);

            CloudNet temp = go.GetComponent<CloudNet>();
            temp.SetIsMain(_b);
            temp.playerToFollow = _b ? player : player.otherPlayer;

            NetworkServer.Spawn(go);
        }

        t_height += 4;

        if (_b)
            MaxHeight2 = t_height;
        else
            MaxHeight4 = t_height;
    }

    public void SaveStats(int cloudsTouched)
    {
        ui.gameObject.SetActive(true);
        ui.SetTexts(player.CloudsTouched.ToString(), ((int)(MaxHeight - StartingPos)).ToString());
        StaticManager.netUtilities.UpdateScore(PlayerPrefs.GetString("username"), player.CloudsTouched, (int)(MaxHeight - StartingPos));
        Debug.Log("Savestats");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
