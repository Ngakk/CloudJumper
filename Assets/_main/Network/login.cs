using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class login : MonoBehaviour
{
    public string usuario;
    public string password;

    private void Start()
    {
        StartCoroutine(IELogin());
    }

    IEnumerator IELogin()
    {
        //WWWForm permite mandar datos a paginas web
        WWWForm sendData = new WWWForm();
        //Como se espera en POST del php ,, el dato o informacion a mandar
        sendData.AddField("username", usuario);
        sendData.AddField("assword", password);

        //Mandamos informacion

        //using (UnityWebRequest www = UnityWebRequest.Post("http://virtualmeatball.rf.gd/games/cloud-jumper/php/login.php", sendData))
        using (UnityWebRequest www = UnityWebRequest.Get("http://virtualmeatball.rf.gd//test.php"))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
}
