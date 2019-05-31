using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetUtilities : MonoBehaviour
{
    char splitter = '◙';

    bool waitingResponse = false;

    private void Awake()
    {
        StaticManager.netUtilities = this;
    }

    public void Register(string _username, string _password, Action<bool,string> _callback)
    {
        if (waitingResponse)
            return;

        StartCoroutine(IERegister(_username, _password, _callback));
    }

    public void Login(string _username, string _password, Action<bool, string> _callback)
    {
        if (waitingResponse)
            return;

        StartCoroutine(IELogin(_username, _password, _callback));
    }

    public void Leaderboard(int _page, Action<string[]> _callback)
    {
        if (waitingResponse)
            return;

        StartCoroutine(IELeaderboard(_page, _callback));
    }

    public void UpdateScore(string _username, int _cloud, int _height)
    {
        if (waitingResponse)
            return;

        StartCoroutine(IEScore(_username, _cloud, _height));
    }

    IEnumerator IERegister(string _username, string _password, Action<bool, string> _callback)
    {
        waitingResponse = true;

        WWWForm sendData = new WWWForm();

        sendData.AddField("username", _username);
        sendData.AddField("password", _password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://virtualmeatball.mygamesonline.org/games/cloud_jumper/php/register.php", sendData))
        {
            Debug.Log("Connecting...");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                _callback.Invoke(false, "There was an error processing your request");
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.downloadHandler.text);

                string[] response = www.downloadHandler.text.Split(splitter);

                if (response[0] == "success")
                {
                    _callback.Invoke(true, String.Empty);
                }
                else
                {
                    _callback.Invoke(false, "Duplicate username");
                }
            }
        }

        waitingResponse = false;
    }

    IEnumerator IELogin(string _username, string _password, Action<bool, string> _callback)
    {
        waitingResponse = true;

        //WWWForm permite mandar datos a paginas web
        WWWForm sendData = new WWWForm();
        //Como se espera en POST del php ,, el dato o informacion a mandar
        sendData.AddField("username", _username);
        sendData.AddField("password", _password);

        //Mandamos informacion

        using (UnityWebRequest www = UnityWebRequest.Post("http://virtualmeatball.mygamesonline.org/games/cloud_jumper/php/login.php", sendData))
        {
            Debug.Log("Connecting...");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                _callback.Invoke(false, "There was an error processing your request");
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.downloadHandler.text);

                string[] response = www.downloadHandler.text.Split(splitter);

                if (response[0].Equals("success"))
                {
                    PlayerPrefs.SetString("username", response[1]);

                    _callback.Invoke(true, "Login succesfull");
                }
                else
                {
                    _callback.Invoke(false, response[1]);
                }
            }
        }

        waitingResponse = false;
    }
    
    IEnumerator IELeaderboard(int _page, Action<string[]> _callback)
    {
        waitingResponse = true;

        //WWWForm permite mandar datos a paginas web
        WWWForm sendData = new WWWForm();
        //Como se espera en POST del php ,, el dato o informacion a mandar
        sendData.AddField("page", _page);

        //Mandamos informacion

        using (UnityWebRequest www = UnityWebRequest.Post("http://virtualmeatball.mygamesonline.org/games/cloud_jumper/php/top_scores.php", sendData))
        {
            Debug.Log("Connecting...");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
                _callback.Invoke(new string[] {"empty"});
            }
            else
            {
                Debug.Log("Form upload complete!");

                string[] response = www.downloadHandler.text.Split(splitter);

                _callback.Invoke(response);
            }
        }

        waitingResponse = false;
    }

    IEnumerator IEScore(string _username, int _score, int _height)
    {
        waitingResponse = true;

        WWWForm sendData = new WWWForm();

        sendData.AddField("username", _username);
        sendData.AddField("score", _score);
        sendData.AddField("height", _height);

        using (UnityWebRequest www = UnityWebRequest.Post("http://virtualmeatball.mygamesonline.org/games/cloud_jumper/php/update_score.php", sendData))
        {
            Debug.Log("Connecting...");
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Update score fomr load complete");
            }
        }

        waitingResponse = false;
    }
}
