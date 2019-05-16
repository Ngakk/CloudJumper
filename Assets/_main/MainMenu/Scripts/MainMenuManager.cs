using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public NetUtilities netManager;

    public InputField username, password;

    public void Login()
    {
        netManager.Login(username.text, password.text, OnLogin);
    }

    public void Register()
    {
        netManager.Register(username.text, password.text, OnRegister);
    }

    public void OnLogin(bool _success, string _message)
    {
        if (_success)
            Debug.Log("YOU JUST LOGED ING SUCCESFULLY WOOOOO, HELLO " + _message);
        else
            Debug.Log("WTF ERROR: " + _message);
    }

    public void OnRegister(bool _success, string _message)
    {

    }
}
