using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public NetUtilities netManager;

    public GameObject PanelA, PanelLogin, PanelLogedIn, PanelForm, TextPrefab;

    public Transform LBUser, LBScore, LBHeight;

    public InputField username, password;

    public Text error, welcome;

    public void Start()
    {
        if(PlayerPrefs.HasKey("username"))
        {
            if (PlayerPrefs.GetString("username") != "")
            {
                PanelA.SetActive(false);
                PanelLogedIn.SetActive(true);
                welcome.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
            }
        }
    }

    public void Login()
    {
        netManager.Login(username.text, password.text, OnLogin);
    }

    public void OnLogin(bool _success, string _message)
    {
        if (_success)
        {
            Debug.Log("YOU JUST LOGED ING SUCCESFULLY WOOOOO, HELLO " + _message);
            PanelLogin.SetActive(false);
            PanelForm.SetActive(false);
            PanelLogedIn.SetActive(true);
            welcome.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
        }
        else
        {
            Debug.Log("WTF ERROR: " + _message);
            error.text = _message;
        }
    }

    public void LogOut()
    {
        PlayerPrefs.SetString("username", "");
    }

    public void OnFormOpen()
    {
        error.text = "";
        username.text = "";
        password.text = "";
    }

    public void Register()
    {
        netManager.Register(username.text, password.text, OnRegister);
    }

    public void OnRegister(bool _success, string _message)
    {
        if (_success)
        {
            error.text = "Registration complete";
        }
        else
        {
            error.text = "ERROR: " + _message;
        }
    }

    public void Leaderboard()
    {
        netManager.Leaderboard(0, OnLeaderboard);
    }

    public void OnLeaderboard(string[] _data)
    {
        ClearLeaderboard();

        if (_data[0] == "empty")
        {

        }
        else
        {
            for (int i = 0; i+3 < _data.Length; i+=3)
            {
                GameObject new_u = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity, LBUser);
                GameObject new_s = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity, LBScore);
                GameObject new_h = Instantiate(TextPrefab, Vector3.zero, Quaternion.identity, LBHeight);

                new_u.GetComponent<Text>().text = _data[i];
                new_u.GetComponent<Text>().rectTransform.localPosition = Vector3.down * (Mathf.Floor(i / 3)) * 30.0f;
                new_s.GetComponent<Text>().text = _data[i+1];
                new_s.GetComponent<Text>().rectTransform.localPosition = Vector3.down * (Mathf.Floor(i / 3)) * 30.0f;
                new_h.GetComponent<Text>().text = _data[i+2];
                new_h.GetComponent<Text>().rectTransform.localPosition = Vector3.down * (Mathf.Floor(i / 3)) * 30.0f;
            }
        }
    }

    private void ClearLeaderboard()
    {
        foreach (RectTransform child in LBUser.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (RectTransform child in LBScore.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (RectTransform child in LBHeight.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartGame()
    {
        DontDestroyOnLoad(netManager.gameObject);
        SceneManager.LoadScene(1);
    }

    public void StartMultiplayer()
    {
        DontDestroyOnLoad(netManager.gameObject);
        SceneManager.LoadScene(2);
    }
}
