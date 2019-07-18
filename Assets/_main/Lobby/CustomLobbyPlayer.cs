using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomLobbyPlayer : NetworkLobbyPlayer
{ 
    public Toggle tgl_ready;
    public Text txt_Jugador;

    private void Awake()
    {
    }

    public override void OnClientEnterLobby()
    {
        print("OnClientEnterLobby");
        transform.SetParent(GameObject.Find("Canvas").transform, false);

        //agregamos listener al UI
        tgl_ready.onValueChanged.AddListener(CambioReady);
        //actualizamos nombre de jugador 
        txt_Jugador.text = "Player " + (slot + 1).ToString();
        OnClientReady(false);
    }

    public override void OnClientExitLobby()
    {
        print("OnClientExitLobby");
    }

    public override void OnClientReady(bool readyState)
    {
        print("OnClientReady");
        //actualizamos la version local representaticvo
        tgl_ready.isOn = readyState;
        if(readyState)
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnStartClient()
    {
        //all networkbehaviour base function don't do anything
        //but NetworkLobbyPlayer redefine OnStartClient, so we need to call it here
        base.OnStartClient();
        print("OnStartClient");
        //setup the player data on UI. The value are SyncVar so the player
        //will be created with the right value currently on server
    }

    public override void OnStartLocalPlayer()
    {
        print("OnStartLocalPlayer");
        //activamos solo la version local
        tgl_ready.interactable = true;
        base.OnStartLocalPlayer();
    }

    public void CambioReady(bool _set)
    {
        //cambiamos para avisarle a los demas, pero solo si es local
        if (!base.isLocalPlayer) return;

        if (_set)
            base.SendReadyToBeginMessage();
        else
            base.SendNotReadyToBeginMessage();
    }
}
