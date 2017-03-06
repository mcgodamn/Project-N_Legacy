using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class CharController : NetworkBehaviour
{
    public Vector2 moveDirection;
    public int walldirection;
    player player;
    CharacterController controller;
    public float WalkSpeed;
    public float JumpSpeed;
    public bool bf;
    public bool blockr;
    public bool blockl;
    public char skystatus;
    public float gravity;
    public string hitname;
    GameObject player2;
	int weapon;
    // Use this for initialization
    private void Awake()
    {
        moveDirection = new Vector2();
        player = GetComponent<player>();
        controller = GetComponent<CharacterController>();
        if (isLocalPlayer && player.PlayerNum) GameManager.Instance.NowPlayer = 1;
        else if (isLocalPlayer && !player.PlayerNum) GameManager.Instance.NowPlayer = 2;
		weapon = 0;
    }
    public void Start () {
        bf = false; blockr = false; blockl = false;
        skystatus = 'g';
        walldirection = 0;
        moveDirection = new Vector2();
		if (!Network.isServer)
			CmdStand();
		else RpcStand();
    }

    // Update is called once per frame
    void Update()
    {
        if (player2 == null)
        {
            if (tag == "1p" && !GameManager.Instance.player) return;
            if (player.PlayerNum) player2 = GameObject.FindWithTag("2p");
            else player2 = GameObject.FindWithTag("1p");
			if (player2 != null) {
				Physics.IgnoreCollision(player2.GetComponent<Collider>(), GetComponent<Collider>());
				StartCoroutine (GameManager.Instance.StartCount());
			}
        }
        if (!isLocalPlayer)
        {
            return;
        }
        float x;
        if (skystatus == 'g')
        {
            x = moveDirection.x;
            if (!bf && controller.isGrounded)
            {
                if(GameManager.Instance.Gaming) moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
                if (moveDirection != Vector2.zero) player.slashing = false;
                moveDirection = transform.TransformDirection(moveDirection);
                if (x != moveDirection.x)
                {
                    x = moveDirection.x;
                    if (!Network.isServer)
                    {
                        if (x > 0) CmdRight();
                        else if (x < 0) CmdLeft();
                        else CmdStand();
                    }
                    else
                    {
                        if (x > 0) RpcRight();
                        else if (x < 0) RpcLeft();
                        else RpcStand();
                    }
                }
                moveDirection *= WalkSpeed;
                if (Input.GetButton("Jump")) moveDirection.y = JumpSpeed;
            }
            if(bf) moveDirection.x = 0;
            moveDirection.y -= gravity * Time.deltaTime;
			Debug.Log ("M");
			if(GameManager.Instance.Gaming || !controller.isGrounded) controller.Move(moveDirection * Time.deltaTime);
        }
        else if (skystatus == 'w')
        {
            if (!Network.isServer)
                CmdWall();
            else RpcWall();
        }
        else if (skystatus == 's')
        {
			if (weapon == 0) {
				if (!Network.isServer)
					CmdSlash ();
				else
					RpcSlash ();
			} 
			else if (weapon == 1) {
				if (!Network.isServer)
					CmdSlash ();
				else
					RpcSlash ();
			}
        }
        else if(skystatus == 'c')
        {
            if (!Network.isServer)
                CmdClose();
            else RpcClose();
        }

        if (GameManager.Instance.Gaming && Input.GetButtonDown("slash") && !blockr && !blockl && skystatus != 'c')
        {

			if (!Network.isServer)
				CmdChangeAttackP();
			else RpcChangeAttackP();
            skystatus = 's';
        }

        if (GameManager.Instance.Gaming && Input.GetButton("block"))
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                Debug.Log("blockR");
                bf = true;
                blockr = true;
                blockl = false;
                if (!Network.isServer)
                    CmdBright();
                else RpcBright();
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                Debug.Log("blockL");
                bf = true;
                blockr = false;
                blockl = true;
                if (!Network.isServer)
                    CmdBleft();
                else RpcBleft();
            }
        }

        if (GameManager.Instance.Gaming && Input.GetButtonUp("block") && isLocalPlayer)
        {
			if (!Network.isServer)
				CmdStand();
			else
				RpcStand();
            blockr = false;
            blockl = false;
            bf = false;
        }
        if(GameManager.Instance.Gaming)
        {
            if (Input.GetAxis("Vertical") > 0.4 && Input.GetAxis("Horizontal") > 0)
            {
                if (Input.GetButton("wall"))
                {
                    if (walldirection != 2)
                    {
                        if (!Network.isServer)
                            CmdRight();
                        else RpcRight();
                        if (!Network.isServer)
                            CmdChangeWD(2);
                        else RpcChangeWD(2);
                        skystatus = 'w';
                    }
                }
            }
            else if (Input.GetAxis("Vertical") > 0.4 && Input.GetAxis("Horizontal") < 0)
            {
                if (Input.GetButton("wall"))
                {
                    if (walldirection != 8)
                    {
                        if (!Network.isServer)
                            CmdLeft();
                        else RpcLeft();
                        if (!Network.isServer)
                            CmdChangeWD(8);
                        else RpcChangeWD(8);
                        skystatus = 'w';
                    }
                }
            }
            else if (Input.GetAxis("Vertical") < -0.4 && Input.GetAxis("Horizontal") > 0)
            {
                if (Input.GetButton("wall"))
                {
                    if (!controller.isGrounded)
                    {
                        if (walldirection != 4)
                        {
                            if (!Network.isServer)
                                CmdRight();
                            else RpcRight();
                            if (!Network.isServer)
                                CmdChangeWD(4);
                            else RpcChangeWD(4);
                            skystatus = 'w';
                        }
                    }
                }
            }
            else if (Input.GetAxis("Vertical") < -0.4 && Input.GetAxis("Horizontal") < 0)
            {
                if (Input.GetButton("wall"))
                {
                    if (!controller.isGrounded)
                    {
                        if (walldirection != 6)
                        {
                            if (!Network.isServer)
                                CmdLeft();
                            else RpcLeft();
                            if (!Network.isServer)
                                CmdChangeWD(6);
                            else RpcChangeWD(6);
                            skystatus = 'w';
                        }
                    }
                }
            }
            else if (Input.GetAxis("Horizontal") > 0)
            {
                if (Input.GetButton("wall"))
                {
                    if (walldirection != 3)
                    {
                        if (!Network.isServer)
                            CmdRight();
                        else RpcRight();
                        if (!Network.isServer)
                            CmdChangeWD(3);
                        else RpcChangeWD(3);
                        skystatus = 'w';
                    }
                }
            }
            else if (Input.GetAxis("Vertical") > 0.4)
            {
                if (Input.GetButton("wall"))
                {
                    if (walldirection != 1)
                    {
                        if (!Network.isServer)
                            CmdChangeWD(1);
                        else RpcChangeWD(1);
                        skystatus = 'w';
                    }
                }
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                if (Input.GetButton("wall"))
                {
                    if (walldirection != 7)
                    {
                        if (!Network.isServer)
                            CmdLeft();
                        else RpcLeft();
                        if (!Network.isServer)
                            CmdChangeWD(7);
                        else RpcChangeWD(7);
                        skystatus = 'w';
                    }
                }
            }
            else if (Input.GetAxis("Vertical") < -0.4)
            {
                if (Input.GetButton("wall"))
                {
                    if (!controller.isGrounded)
                    {
                        if (walldirection != 5)
                        {
                            if (!Network.isServer)
                                CmdChangeWD(5);
                            else RpcChangeWD(5);
                            skystatus = 'w';
                        }
                    }
                }
            }
        }
    }

    [Command] //ONLY BY CLINT
    public void CmdBright()
    {
        RpcBright();
    }
    [ClientRpc] // ONLY BY SERVER
    public void RpcBright()
    {
        player.bright();
    }
    [Command]
    public void CmdBleft()
    {
        RpcBleft();
    }
    [ClientRpc]
    public void RpcBleft()
    {
        player.bleft();
    }

    [Command]
    public void CmdRight()
    {
        RpcRight();
    }
    [ClientRpc]
    public void RpcRight()
    {
        player.right();
    }
    [Command]
    public void CmdChangeWD(int i)
    {
        RpcChangeWD(i);
    }
    [ClientRpc]
    public void RpcChangeWD(int i)
    {
        walldirection = i;
    }
    [Command]
    public void CmdLeft()
    {
        RpcLeft();
    }
    [ClientRpc]
    public void RpcLeft()
    {
        player.left();
    }

    [Command]
    public void CmdStand()
    {
        RpcStand();
    }
    [ClientRpc]
    public void RpcStand()
    {
        player.stand();
    }

    [Command]
    public void CmdWall()
    {
        RpcWall();
    }
    [ClientRpc]
    public void RpcWall()
    {
        player.wall(walldirection,isLocalPlayer);
    }

    [Command]
    public void CmdLRUnEnable()
    {
        RpcLRUnEnable();
    }
    [ClientRpc]
    public void RpcLRUnEnable()
    {
        player.lrunable();
    }

    [Command]
    public void CmdSlash()
    {
        RpcSlash();
    }
    [ClientRpc]
    public void RpcSlash()
    {
        player.slash();
    }

	[Command]
	public void CmdShuriken()
	{
		RpcShuriken();
	}
	[ClientRpc]
	public void RpcShuriken()
	{
		player.slash();
	}

    [Command]
    public void CmdClose()
    {
        RpcClose();
    }
    [ClientRpc]
    public void RpcClose()
    {
        player.close();
    }
    public void FixedUpdate()
    {
        if (controller.isGrounded && skystatus != 's')
        {
            if (!(skystatus == 'w' && (walldirection == 3 || walldirection == 7)))
            {
                if (!Network.isServer)
                    CmdLRUnEnable();
                else RpcLRUnEnable();
                skystatus = 'g';
                walldirection = 0;
            }
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hitname != hit.gameObject.name)
        {
            walldirection = 0;
            if (!Network.isServer)
                CmdLRUnEnable();
            else RpcLRUnEnable();
            hitname = hit.gameObject.name;
        }
    }

    public void Win()
    {
        if (!Network.isServer)
            CmdWin(GameManager.Instance.Name);
        else RpcWin(GameManager.Instance.Name);
    }

    public void RestartGame()
    {
        if (!Network.isServer)
            CmdReGame();
        else RpcReGame();
    }

    [Command]
    void CmdReGame()
    {
        RpcReGame();
    }

    [ClientRpc]
    void RpcReGame()
    {
        GameManager.Instance.ReGame();
    }

    [Command]
    void CmdWin(string str)
    {
        RpcWin(str);
    }

    [ClientRpc]
    void RpcWin(string str)
    {
        GameManager.Instance.Gaming = false;
        GameManager.Instance.InGame.SetActive(true);
        GameManager.Instance.InGameText.GetComponent<Text>().text = str + " Win!!";
    }

	[Command]
	void CmdChangeAttackP()
	{
		RpcChangeAttackP();
	}

	[ClientRpc]
	void RpcChangeAttackP()
	{
		player.attacking = true;
		GameManager.Instance.LastAttackPlayer = tag;
	}
}
