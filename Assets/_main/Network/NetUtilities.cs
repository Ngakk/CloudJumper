using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetUtilities : MonoBehaviour
{
    int id;
    string usuario;
    char splitter = '◙';

    bool waitingResponse = false;

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

                if (www.downloadHandler.text == "success")
                {
                    _callback.Invoke(true, String.Empty);
                }
                else
                {
                    _callback.Invoke(false, "We did something wrong i guess");
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
                    id = int.Parse(response[1]);
                    usuario = response[2];

                    _callback.Invoke(true, response[1]);
                }
                else
                {
                    _callback.Invoke(false, response[1]);
                }
            }
        }

        waitingResponse = false;
    }
}
