using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class register : MonoBehaviour
{
    public string usuario;
    public string password;

    private void Start()
    {
        StartCoroutine(IERegister());
    }

    IEnumerator IERegister()
    {
        WWWForm sendData = new WWWForm();

        sendData.AddField("username", usuario);
        sendData.AddField("password", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://virtualmeatball.mygamesonline.org/games/cloud_jumper/php/register.php", sendData))
        //using (UnityWebRequest www = UnityWebRequest.Get("http://virtualmeatball.mygamesonline.org/games/cloud_jumper/php/register.php"))
        {
            Debug.Log("Connecting...");
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
