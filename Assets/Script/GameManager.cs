using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkManager {
    public static GameManager Instance;
    public bool player; // true for p1 ,false for p2
    public int NowPlayer;
    public bool Gaming;
    public int port;
    public string Name;
    public GameObject menu, IpAddress, InGame, PlayerName, InGameText;
    // Use this for initialization
    void Start ()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(this);
        DontDestroyOnLoad(this);
        player = true; Gaming = false;
        menu.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        Name = PlayerName.GetComponent<InputField>().text;
        menu.SetActive(false);
        SetPort();
        singleton.StartHost();
    }

    public void JoinGame()
    {
        SetPort();
        if (SetIPAddress())
        {
            menu.SetActive(false);
            singleton.StartClient();
        }
    }

    bool SetIPAddress()
    {
        string ipAddress = IpAddress.GetComponent<InputField>().text;
        if (ipAddress == "")
        {
            singleton.networkAddress = "localhost";
            //IpAddress.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Pleace enter IP!!";
            //return false;
        }
        else singleton.networkAddress = ipAddress;
        return true;
    }

    void SetPort()
    {
        singleton.networkPort = port;
    }

    public void ReGameButton()
    {
        GameObject player = GameObject.FindGameObjectWithTag("1p");
        player.GetComponent<CharController>().RestartGame();
    }

    public void ReGame()
    {
        InGame.SetActive(false);
        GameObject[] player = new GameObject[2];
        player[0] = GameObject.FindGameObjectWithTag("1p");
        player[1] = GameObject.FindGameObjectWithTag("2p");
        player[0].GetComponent<player>().Start();
        player[0].GetComponent<CharController>().Start();
        player[1].GetComponent<player>().Start();
        player[1].GetComponent<CharController>().Start();
        Gaming = true;
    }
}
