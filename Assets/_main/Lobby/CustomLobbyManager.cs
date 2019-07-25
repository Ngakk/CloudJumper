using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class CustomLobbyManager : NetworkLobbyManager
{
    void Start()
    {
        CStart();
        CListaSalas();
    }

    void CStart()
    {
        print("se inicio");
        StartMatchMaker(); //Inicializa
    }

    void CListaSalas()
    {
        print("Listas");

        matchMaker.ListMatches(0, 10, "", true, 0, 0, OnMatchList);
    }

    /*public override void OnClientSceneChanged(NetworkConnection conn)
    {
        
    }
    */
    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        print("OnMatchList");
        base.OnMatchList(success, extendedInfo, matchList);

        if(success)
        {
            print("Numero de salas: " + matchList);

            if (matchList.Count > 0)
            {
                print("Hay salas disponibles ");
                print("Nombre de la sala: " + matchList[0].name + " y su ID: " + matchList[0].networkId);
                CUnirSala(matchList[0]); //Tratamos de unirnos a la primera sala
            }
            else //No hay salas creadas
            {
                CCrearSala(); //Intentamos crear 
            }
        }
        else
        {
            print("Error OnMatchList: " + extendedInfo);
        }
    }

    void CUnirSala(UnityEngine.Networking.Match.MatchInfoSnapshot _sala)
    {
        print("Unir a sala");
        matchMaker.JoinMatch(_sala.networkId, "", "", "", 0, 0, OnMatchJoined);
    }

    void CCrearSala()
    {
        print("Crear sala"); //Nombre sala, jugadores, es publica, contraseña
        matchMaker.CreateMatch("SalaCustom2", 4, true, "", "", "", 0, 0, OnMatchCreate);
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        print("OnMatchJoined");
        base.OnMatchJoined(success, extendedInfo, matchInfo);

        print("Unido y estoy en la escena: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        if(success)
        {
            print("Union exitosa " + extendedInfo);
        }
        else
        {
            print("Error al unir: " + extendedInfo);
        }

    }

    public override void OnLobbyServerPlayersReady()
    {
        print("OnLobbyServerPlayersReady-------------------------------3");
        base.OnLobbyServerPlayersReady();
    }

    public void IniciarPartida()
    {
        base.ServerChangeScene("Game");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1) && NetworkServer.active)
        {
            print("Iniciamos partiada");
            IniciarPartida();
        }
    }

    //Se llama cuando se genera el lobby player (SOLO EN SERVIDOR)
    public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId)
    {
        return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
    }

    public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId)
    {
        print("OnLobbyServerCreatedGamePlayer---------------------");
        GameObject obj = Instantiate(gamePlayerPrefab) as GameObject;

        return obj;
    }

    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        Debug.Log("OnLobbyServerSceneLoadedForPlayer--------------------------2");

        NetworkConnection conn = lobbyPlayer.GetComponent<CustomLobbyPlayer>().connectionToClient;

        gamePlayer.transform.position = Vector3.zero;

        return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer, gamePlayer);
    }
}
