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
    public string Name,LastAttackPlayer;
    public GameObject menu, IpAddress, InGame, PlayerName, InGameText, MyCharacter,Count,CountText;
	public GameObject character;
	public bool solo;
	GameObject p1, p2;
	SpriteRenderer p1color,p2color;
	Coroutine Stand;
	float StartTime;
	public Color FadeInColor;
    // Use this for initialization
    void Start ()
    {
        if (Instance == null) Instance = this;
        if (Instance != this) Destroy(this);
        DontDestroyOnLoad(this);
        player = true; Gaming = false;
        menu.SetActive(true);
		solo = false;
		FadeInColor = new Color (1, 1, 1, 0);
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
		if (!solo)
			MyCharacter.GetComponent<CharController> ().RestartGame ();
		else {
			SoloMode ();
			InGame.SetActive (false);
		}
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
		StartCoroutine(StartCount ());
    }

	public void SoloMode()
	{
		menu.SetActive (false);
		GetComponent<SoloMode> ().enabled = true;
		if (solo) {
			p1 = GameObject.FindGameObjectWithTag ("1p");
			p2 = GameObject.FindGameObjectWithTag ("2p");
			p1.transform.position = new Vector3 (-4, -3.565f, 0);
			p2.transform.position = new Vector3 (4, -3.565f, 0);
		} else {
			p1 = (GameObject)Instantiate(character, new Vector3(-4, -3.565f, 0), new Quaternion());
			p2 = (GameObject)Instantiate(character, new Vector3(4, -3.565f, 0), new Quaternion());
		}
		p1color = p1.GetComponent<SpriteRenderer> ();
		p2color = p2.GetComponent<SpriteRenderer> ();
		StartTime = Time.time;
		if(!solo)StartCoroutine (FadeIn());
		solo = true;
		p1.GetComponent<player> ().enabled = false;
		p2.GetComponent<player> ().enabled = false;
		p1.GetComponent<CharController> ().enabled = false;
		p2.GetComponent<CharController> ().enabled = false;
		GetComponent<SoloMode> ().AsignPlayer (p1, p2);
		StartCoroutine(StartCount ());
		//Stand = StartCoroutine (standup ());
	}
	public IEnumerator standup()
	{
		while (true) {
			GetComponent<SoloMode> ().stand ();
			GetComponent<SoloMode> ().stand2 ();
		}
		yield return 0;
	}

	public IEnumerator StartCount()
	{
		yield return new WaitForSeconds (0.7f);
		Count.SetActive (true);
		CountText.GetComponent<Text> ().text = 3 +"";
		float now = Time.time;
		for (int i = 2; i > -1; i--) {
			//if (i == 1)
				//StopCoroutine (Stand);
			yield return new WaitForSeconds(1);
			CountText.GetComponent<Text> ().text = i +"";
		}
		Count.SetActive (false);
		Gaming = true;
	}

	public IEnumerator FadeIn()
	{
		while (true) {
			FadeInColor.a = (Time.time - StartTime) / 3;
			if (FadeInColor.a > 1)
				break;
			p1color.color = FadeInColor;
			p2color.color = FadeInColor;
			yield return null;
		}
		p1color.color = Color.white;
		p2color.color = Color.white;
	}
}
