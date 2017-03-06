using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SoloMode : MonoBehaviour {
	public GameObject p1, p2;
	public Vector2 moveDirection,moveDirection2;
	public int walldirection,walldirection2;
	CharacterController controller,controller2;
	public float WalkSpeed;
	public float JumpSpeed;
	public bool bf,bf2;
	public bool blockr,blockr2;
	public bool blockl,blockl2;
	public char skystatus,skystatus2;
	public float gravity;
	public string hitname,hitname2;

	public float wallspeed;
	public float slashspeed;
	public bool WalkReturn,WalkReturn2;
	public Sprite[] PlayerSprite;
	public Sprite BasicSprite,BasicSprite2;
	public int ci,ci2;
	public bool slashing,slashing2,attacking,attacking2;
	LineRenderer lr,lr2;

	private void Awake()
	{
		moveDirection = new Vector2();
	}
	// Use this for initialization
	void Start () {
		bf = false; blockr = false; blockl = false; bf2 = false; blockr2 = false; blockl2 = false;
		skystatus = 'g'; skystatus2 = 'g';
		walldirection = 0; walldirection2 = 0;
		moveDirection = new Vector2(); moveDirection2 = new Vector2();
		attacking = false; attacking2 = false;
		slashing = false; slashing2 = false;
		WalkReturn = true; WalkReturn2 = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (p1 == null || p2 == null)
			return;
		float x,x2;

		//player1 part
		if (skystatus == 'g')
		{
			x = moveDirection.x;
			if (!bf && controller.isGrounded)
			{
				if(GameManager.Instance.Gaming) moveDirection = new Vector2(Input.GetAxis("Horizontal"), 0);
				if (moveDirection != Vector2.zero) slashing = false;
				moveDirection = p1.transform.TransformDirection(moveDirection);
				if (x != moveDirection.x)
				{
					x = moveDirection.x;
					if (x > 0) right();
					else if (x < 0) left();
					else stand();
				}
				moveDirection *= WalkSpeed;
				if (Input.GetButton("Jump")) moveDirection.y = JumpSpeed;
			}
			if(bf) moveDirection.x = 0;
			moveDirection.y -= gravity * Time.deltaTime;
			if(GameManager.Instance.Gaming || !controller.isGrounded) controller.Move(moveDirection * Time.deltaTime);
		}
		else if (skystatus == 'w')
		{
			wall (walldirection);
		}
		else if (skystatus == 's')
		{
			slash ();
		}
		else if(skystatus == 'c')
		{
			close ();
		}

		if (GameManager.Instance.Gaming && Input.GetButtonDown("slash") && !blockr && !blockl && skystatus != 'c')
		{
			attacking = true;
			GameManager.Instance.LastAttackPlayer = p1.tag;
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
				bright ();
			}
			else if (Input.GetAxis("Horizontal") < 0)
			{
				Debug.Log("blockL");
				bf = true;
				blockr = false;
				blockl = true;
				bleft ();
			}
		}

		if (GameManager.Instance.Gaming && Input.GetButtonUp("block"))
		{
			stand ();
			blockr = false;
			blockl = false;
			bf = false;
		}
		if(GameManager.Instance.Gaming)
		{
			if (Input.GetAxis("Vertical") > 0.4f && Input.GetAxis("Horizontal") > 0)
			{
				if (Input.GetButton("wall"))
				{
					if (walldirection != 2)
					{
						right ();
						walldirection = 2;
						skystatus = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical") > 0.4f && Input.GetAxis("Horizontal") < 0)
			{
				if (Input.GetButton("wall"))
				{
					if (walldirection != 8)
					{
						left ();
						walldirection = 8;
						skystatus = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical") < -0.4f && Input.GetAxis("Horizontal") > 0)
			{
				if (Input.GetButton("wall"))
				{
					if (!controller.isGrounded)
					{
						if (walldirection != 4)
						{
							right ();
							walldirection = 4;
							skystatus = 'w';
						}
					}
				}
			}
			else if (Input.GetAxis("Vertical") < -0.4f && Input.GetAxis("Horizontal") < 0)
			{
				if (Input.GetButton("wall"))
				{
					if (!controller.isGrounded)
					{
						if (walldirection != 6)
						{
							left ();
							walldirection = 6;
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
						right ();
						walldirection = 3;
						skystatus = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical") > 0.4f)
			{
				if (Input.GetButton("wall"))
				{
					if (walldirection != 1)
					{
						walldirection = 1;
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
						left ();
						walldirection = 7;
						skystatus = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical") < -0.4f)
			{
				if (Input.GetButton("wall"))
				{
					if (!controller.isGrounded)
					{
						if (walldirection != 5)
						{
							walldirection = 5;
							skystatus = 'w';
						}
					}
				}
			}
		}

		//player2 part
		if (skystatus2 == 'g')
		{
			x2 = moveDirection2.x;
			if (!bf2 && controller2.isGrounded)
			{
				if(GameManager.Instance.Gaming) moveDirection2 = new Vector2(Input.GetAxis("Horizontal2"), 0);
				if (moveDirection2 != Vector2.zero) slashing2 = false;
				moveDirection2 = p2.transform.TransformDirection(moveDirection2);
				if (x2 != moveDirection2.x)
				{
					x2 = moveDirection2.x;
					if (x2 > 0) right2();
					else if (x2 < 0) left2();
					else stand2();
				}
				moveDirection2 *= WalkSpeed;
				if (Input.GetButton("Jump2")) moveDirection2.y = JumpSpeed;
			}
			if(bf2) moveDirection2.x = 0;
			moveDirection2.y -= gravity * Time.deltaTime;
			if(GameManager.Instance.Gaming || !controller2.isGrounded) controller2.Move(moveDirection2 * Time.deltaTime);
		}
		else if (skystatus2 == 'w')
		{
			wall2 (walldirection2);
		}
		else if (skystatus2 == 's')
		{
			slash2 ();
		}
		else if(skystatus2 == 'c')
		{
			close2 ();
		}

		if (GameManager.Instance.Gaming && Input.GetButtonDown("slash2") && !blockr2 && !blockl2 && skystatus2 != 'c')
		{
			attacking2 = true;
			GameManager.Instance.LastAttackPlayer = p2.tag;
			skystatus2 = 's';
		}

		if (GameManager.Instance.Gaming && Input.GetButton("block2"))
		{
			if (Input.GetAxis("Horizontal2") > 0)
			{
				bf2 = true;
				blockr2 = true;
				blockl2 = false;
				bright2 ();
			}
			else if (Input.GetAxis("Horizontal2") < 0)
			{
				bf2 = true;
				blockr2 = false;
				blockl2 = true;
				bleft2 ();
			}
		}

		if (GameManager.Instance.Gaming && Input.GetButtonUp("block2"))
		{
			stand2 ();
			blockr2 = false;
			blockl2 = false;
			bf2 = false;
		}
		if(GameManager.Instance.Gaming)
		{
			if (Input.GetAxis("Vertical2") > 0.4f && Input.GetAxis("Horizontal2") > 0)
			{
				if (Input.GetButton("wall2"))
				{
					if (walldirection2 != 2)
					{
						right2 ();
						walldirection2 = 2;
						skystatus2 = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical2") > 0.4f && Input.GetAxis("Horizontal2") < 0)
			{
				if (Input.GetButton("wall2"))
				{
					if (walldirection2 != 8)
					{
						left2 ();
						walldirection2 = 8;
						skystatus2 = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical2") < -0.4f && Input.GetAxis("Horizontal2") > 0)
			{
				if (Input.GetButton("wall2"))
				{
					if (!controller2.isGrounded)
					{
						if (walldirection2 != 4)
						{
							right2 ();
							walldirection2 = 4;
							skystatus2 = 'w';
						}
					}
				}
			}
			else if (Input.GetAxis("Vertical2") < -0.4f && Input.GetAxis("Horizontal2") < 0)
			{
				if (Input.GetButton("wall2"))
				{
					if (!controller2.isGrounded)
					{
						if (walldirection2 != 6)
						{
							left2 ();
							walldirection2 = 6;
							skystatus2 = 'w';
						}
					}
				}
			}
			else if (Input.GetAxis("Horizontal2") > 0)
			{
				if (Input.GetButton("wall2"))
				{
					if (walldirection2 != 3)
					{
						right2 ();
						walldirection2 = 3;
						skystatus2 = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical2") > 0.4f)
			{
				if (Input.GetButton("wall2"))
				{
					if (walldirection2 != 1)
					{
						walldirection2 = 1;
						skystatus2 = 'w';
					}
				}
			}
			else if (Input.GetAxis("Horizontal2") < 0)
			{
				if (Input.GetButton("wall2"))
				{
					if (walldirection2 != 7)
					{
						left2 ();
						walldirection2 = 7;
						skystatus2 = 'w';
					}
				}
			}
			else if (Input.GetAxis("Vertical2") < -0.4f)
			{
				if (Input.GetButton("wall2"))
				{
					if (!controller2.isGrounded)
					{
						if (walldirection2 != 5)
						{
							walldirection2 = 5;
							skystatus2 = 'w';
						}
					}
				}
			}
		}
	}

	public void AsignPlayer(GameObject thisp1,GameObject thisp2)
	{
		p1 = thisp1;
		p2 = thisp2;
		controller = p1.GetComponent<CharacterController>();
		controller2 = p2.GetComponent<CharacterController>();
		lr = p1.GetComponent<LineRenderer>();
		lr2 = p2.GetComponent<LineRenderer>();
		PlayerSprite = Resources.LoadAll<Sprite>("");
		BasicSprite2 = PlayerSprite[7];
		BasicSprite = PlayerSprite[11];
		p1.GetComponent<SpriteRenderer>().sprite = BasicSprite;
		p2.GetComponent<SpriteRenderer>().sprite = BasicSprite2;
		p1.GetComponent<LineRenderer> ().startColor = Color.red;
		p1.GetComponent<LineRenderer> ().endColor = Color.red;
		p1.tag = "1p";
		p2.GetComponent<LineRenderer> ().startColor = Color.blue;
		p2.GetComponent<LineRenderer> ().endColor = Color.blue;
		p2.tag = "2p";
		p1.GetComponent<PlayerColliderHit> ().enabled = true;
		p2.GetComponent<PlayerColliderHit> ().enabled = true;
		Physics.IgnoreCollision(p1.GetComponent<Collider>(), p2.GetComponent<Collider>());
		Physics.IgnoreCollision(p2.GetComponent<Collider>(), p1.GetComponent<Collider>());
		stand ();stand2 ();
	}

	public void FixedUpdate()
	{
		if (controller.isGrounded && skystatus != 's')
		{
			if (!(skystatus == 'w' && (walldirection == 3 || walldirection == 7)))
			{
				lrunable ();
				skystatus = 'g';
				walldirection = 0;
			}
		}

		if (controller2.isGrounded && skystatus2 != 's')
		{
			if (!(skystatus2 == 'w' && (walldirection2 == 3 || walldirection2 == 7)))
			{
				lrunable2 ();
				skystatus2 = 'g';
				walldirection2 = 0;
			}
		}
	}

	public void stand()
	{
		if (!GameManager.Instance.Gaming)
			return;
		p1.GetComponent<SpriteRenderer>().sprite = BasicSprite;
		controller.radius = 0.38f;
		controller.height = 1.47f;
		controller.center = new Vector3(-0.09f, -0.12f, 0);
		if(controller.isGrounded) p1.transform.position = new Vector3(p1.transform.position .x, -3.565f,0);
		WalkReturn = true;
	}

	public void stand2()
	{
		if (!GameManager.Instance.Gaming)
			return;
		p2.GetComponent<SpriteRenderer>().sprite = BasicSprite2;
		controller2.radius = 0.38f;
		controller2.height = 1.47f;
		controller2.center = new Vector3(-0.09f, -0.12f, 0);
		if(controller2.isGrounded) p2.transform.position = new Vector3(p2.transform.position .x, -3.565f,0);
		WalkReturn2 = true;
	}


	public void rslash()
	{
		p1.GetComponent<SpriteRenderer>().sprite = PlayerSprite[14];
		//else GetComponent<SpriteRenderer>().sprite = PlayerSprite[6];
		controller.radius = 0.53f;
		controller.height = 0.81f;
		controller.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
		WalkReturn = false;
	}
	public void lslash()
	{
		p1.GetComponent<SpriteRenderer>().sprite = PlayerSprite[10];
		//else GetComponent<SpriteRenderer>().sprite = PlayerSprite[3];
		controller.radius = 0.53f;
		controller.height = 0.81f;
		controller.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
		WalkReturn = false;
	}

	public void right()
	{
		if(!slashing)
		{
			p1.GetComponent<SpriteRenderer>().sprite = PlayerSprite[13];
			//else GetComponent<SpriteRenderer>().sprite = PlayerSprite[5];
		}
		controller.radius = 0.53f;
		controller.height = 0.81f;
		controller.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
		WalkReturn = false;
	}

	public void left()
	{
		if(!slashing)
		{
			p1.GetComponent<SpriteRenderer>().sprite = PlayerSprite[9];
			//else GetComponent<SpriteRenderer>().sprite = PlayerSprite[2];
		}
		controller.radius = 0.53f;
		controller.height = 0.81f;
		controller.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
		WalkReturn = false;
	}

	public void bright()
	{
		p1.GetComponent<SpriteRenderer>().sprite = PlayerSprite[12];
		//else GetComponent<SpriteRenderer>().sprite = PlayerSprite[4];
	}

	public void bleft()
	{
		p1.GetComponent<SpriteRenderer>().sprite = PlayerSprite[8];
		//else GetComponent<SpriteRenderer>().sprite = PlayerSprite[1];
	}

	public void wall(int walldirection)
	{
		Vector3 pos,pos2;
		lr.enabled = true;
		if(p1.transform.position.x > -8.3f && p1.transform.position.x < 8.1f && p1.transform.position.y < 4.35f)
		{
			hitname = "";
		}
		if (walldirection == 3)
		{
			pos = new Vector3(p1.transform.position.x + 0.44f, p1.transform.position.y, 2);
			pos2 = new Vector3(p1.transform.position.x + 18.3f, p1.transform.position.y, 2);
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos2);
			moveDirection = Vector2.right;
			moveDirection = p1.transform.TransformDirection(moveDirection);
			moveDirection *= wallspeed;
			controller.Move(moveDirection * Time.deltaTime);
		}
		else if (walldirection == 1)
		{
			pos = new Vector3(p1.transform.position.x, p1.transform.position.y - 0.33f, 2);
			pos2 = new Vector3(p1.transform.position.x,p1.transform.position.y + 18, 2);
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos2);
			moveDirection = Vector2.up;
			moveDirection = p1.transform.TransformDirection(moveDirection);
			moveDirection *= wallspeed;
			controller.Move(moveDirection * Time.deltaTime);
		}
		else if (walldirection == 2)
		{
			pos = new Vector3(p1.transform.position.x + 0.3f, p1.transform.position.y + 0.28f, 2);
			pos2 = new Vector3(p1.transform.position.x + 18.3f, p1.transform.position.y + 18, 2);
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos2);
			moveDirection = Vector2.one;
			moveDirection = p1.transform.TransformDirection(moveDirection);
			moveDirection *= wallspeed;
			if (hitname == "sky" || hitname == "rightside") moveDirection = Vector3.zero;
			controller.Move(moveDirection * Time.deltaTime);
		}
		else if (walldirection == 4)
		{
			if (!controller.isGrounded)
			{
				pos = new Vector3(p1.transform.position.x + 0.51f, p1.transform.position.y - 0.42f, 2);
				pos2 = new Vector3(p1.transform.position.x + 18.3f, p1.transform.position.y - 18, 2);
				lr.SetPosition(0, pos);
				lr.SetPosition(1, pos2);
				moveDirection = new Vector2(1, -1);
				moveDirection = p1.transform.TransformDirection(moveDirection);
				moveDirection *= wallspeed;
				if (hitname == "rightside") moveDirection = Vector3.zero;
				controller.Move(moveDirection * Time.deltaTime);
			}
		}
		else if (walldirection == 5)
		{
			if (!controller.isGrounded)
			{
				pos = new Vector3(p1.transform.position.x, p1.transform.position.y - 0.37f, 2);
				pos2 = new Vector3(p1.transform.position.x, p1.transform.position.y - 18, 2);
				lr.SetPosition(0, pos);
				lr.SetPosition(1, pos2);
				moveDirection = Vector2.down;
				moveDirection = transform.TransformDirection(moveDirection);
				moveDirection *= wallspeed;
				controller.Move(moveDirection * Time.deltaTime);
			}
		}
		else if (walldirection == 6)
		{
			if (!controller.isGrounded)
			{
				pos = new Vector3(p1.transform.position.x - 0.26f, p1.transform.position.y - 0.35f, 2);
				pos2 = new Vector3(p1.transform.position.x - 17.7f, p1.transform.position.y - 18, 2);
				lr.SetPosition(0, pos);
				lr.SetPosition(1, pos2);
				moveDirection = new Vector2(-1, -1);
				moveDirection = p1.transform.TransformDirection(moveDirection);
				moveDirection *= wallspeed;
				if (hitname == "leftside") moveDirection = Vector3.zero;
				controller.Move(moveDirection * Time.deltaTime);
			}
		}
		else if (walldirection == 7)
		{
			pos = new Vector3(p1.transform.position.x - 0.3f, p1.transform.position.y, 2);
			pos2 = new Vector3(p1.transform.position.x - 18, p1.transform.position.y, 2);
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos2);
			moveDirection = Vector2.left;
			moveDirection = p1.transform.TransformDirection(moveDirection);
			moveDirection *= wallspeed;
			controller.Move(moveDirection * Time.deltaTime);
		}
		else if (walldirection == 8)
		{
			pos = new Vector3(p1.transform.position.x - 0.2f, p1.transform.position.y + 0.3f, 2);
			pos2 = new Vector3(p1.transform.position.x - 18, p1.transform.position.y + 18, 2);
			lr.SetPosition(0, pos);
			lr.SetPosition(1, pos2);
			moveDirection = new Vector2(-1, 1);
			moveDirection = p1.transform.TransformDirection(moveDirection);
			moveDirection *= wallspeed;
			if (hitname == "sky" || hitname == "leftside") moveDirection = Vector3.zero;
			controller.Move(moveDirection * Time.deltaTime);
		}
	}

	public void slash()
	{
		float i, j,abs,absi,absj;
		lr.enabled = false;
		i = p1.transform.position.x - p2.transform.position.x;
		j = p1.transform.position.y - p2.transform.position.y;
		if (Mathf.Abs(i) < 1 && Mathf.Abs(j) < 0.81)
		{
			if(!(GameManager.Instance.LastAttackPlayer == p1.tag && attacking2) && ((i < 0 && !blockl2) || (i > 0 && !blockr2) || (i == 0)))
				Win("Red");
			ci = 0;
			skystatus = 'c';
		}
		abs = Mathf.Abs(i) > Mathf.Abs(j) ? Mathf.Abs(i) : Mathf.Abs(j);
		absi = i / abs;
		absj = j / abs;
		if (absi <= 0) rslash();
		else lslash();
		moveDirection = new Vector2(absi, absj);
		moveDirection *= slashspeed;
		controller.Move(-1 * moveDirection * Time.deltaTime);
	}

	public void close()
	{
		slashing = true;
		if (ci > 7)
		{
			ci = -1;
			skystatus = 'g';
			moveDirection = new Vector2(0, 0);
			attacking = false;
		}
		controller.Move(-1 * moveDirection * Time.deltaTime);
		ci++;
	}

	public void lrunable()
	{
		lr.enabled = false;
	}

	//player2 execute

	public void rslash2()
	{
		p2.GetComponent<SpriteRenderer>().sprite = PlayerSprite[6];
		controller2.radius = 0.53f;
		controller2.height = 0.81f;
		controller2.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn2) controller2.Move(new Vector2(0, -0.56f));
		WalkReturn2 = false;
	}
	public void lslash2()
	{
		p2.GetComponent<SpriteRenderer>().sprite = PlayerSprite[3];
		controller2.radius = 0.53f;
		controller2.height = 0.81f;
		controller2.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn2) controller2.Move(new Vector2(0, -0.56f));
		WalkReturn2 = false;
	}

	public void right2()
	{
		if(!slashing2)
		{
			p2.GetComponent<SpriteRenderer>().sprite = PlayerSprite[5];
		}
		controller2.radius = 0.53f;
		controller2.height = 0.81f;
		controller2.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn2) controller2.Move(new Vector2(0, -0.56f));
		WalkReturn2 = false;
	}

	public void left2()
	{
		if(!slashing2)
		{
			p2.GetComponent<SpriteRenderer>().sprite = PlayerSprite[2];
		}
		controller2.radius = 0.53f;
		controller2.height = 0.81f;
		controller2.center = new Vector3(0.09f, 0.03f, 0);
		if (WalkReturn2) controller2.Move(new Vector2(0, -0.56f));
		WalkReturn2 = false;
	}

	public void bright2()
	{
		p2.GetComponent<SpriteRenderer>().sprite = PlayerSprite[4];
	}

	public void bleft2()
	{
		p2.GetComponent<SpriteRenderer>().sprite = PlayerSprite[1];
	}

	public void wall2(int walldirection)
	{
		Vector3 pos,pos2;
		lr2.enabled = true;
		if(p2.transform.position.x > -8.3f && p2.transform.position.x < 8.1f && p2.transform.position.y < 4.35f)
		{
			hitname2 = "";
		}
		if (walldirection == 3)
		{
			pos = new Vector3(p2.transform.position.x + 0.44f, p2.transform.position.y, 2);
			pos2 = new Vector3(p2.transform.position.x + 18.3f, p2.transform.position.y, 2);
			lr2.SetPosition(0, pos);
			lr2.SetPosition(1, pos2);
			moveDirection2 = Vector2.right;
			moveDirection2 = p2.transform.TransformDirection(moveDirection2);
			moveDirection2 *= wallspeed;
			controller2.Move(moveDirection2 * Time.deltaTime);
		}
		else if (walldirection == 1)
		{
			pos = new Vector3(p2.transform.position.x, p2.transform.position.y - 0.33f, 2);
			pos2 = new Vector3(p2.transform.position.x,p2.transform.position.y + 18, 2);
			lr2.SetPosition(0, pos);
			lr2.SetPosition(1, pos2);
			moveDirection2 = Vector2.up;
			moveDirection2 = p2.transform.TransformDirection(moveDirection2);
			moveDirection2 *= wallspeed;
			controller2.Move(moveDirection2 * Time.deltaTime);
		}
		else if (walldirection == 2)
		{
			pos = new Vector3(p2.transform.position.x + 0.3f, p2.transform.position.y + 0.28f, 2);
			pos2 = new Vector3(p2.transform.position.x + 18.3f, p2.transform.position.y + 18, 2);
			lr2.SetPosition(0, pos);
			lr2.SetPosition(1, pos2);
			moveDirection2 = Vector2.one;
			moveDirection2 = p2.transform.TransformDirection(moveDirection2);
			moveDirection2 *= wallspeed;
			if (hitname2 == "sky" || hitname2 == "rightside") moveDirection2 = Vector3.zero;
			controller2.Move(moveDirection2 * Time.deltaTime);
		}
		else if (walldirection == 4)
		{
			if (!controller2.isGrounded)
			{
				pos = new Vector3(p2.transform.position.x + 0.51f, p2.transform.position.y - 0.42f, 2);
				pos2 = new Vector3(p2.transform.position.x + 18.3f, p2.transform.position.y - 18, 2);
				lr2.SetPosition(0, pos);
				lr2.SetPosition(1, pos2);
				moveDirection2 = new Vector2(1, -1);
				moveDirection2 = p2.transform.TransformDirection(moveDirection2);
				moveDirection2 *= wallspeed;
				if (hitname2 == "rightside") moveDirection2 = Vector3.zero;
				controller2.Move(moveDirection2 * Time.deltaTime);
			}
		}
		else if (walldirection == 5)
		{
			if (!controller2.isGrounded)
			{
				pos = new Vector3(p2.transform.position.x, p2.transform.position.y - 0.37f, 2);
				pos2 = new Vector3(p2.transform.position.x, p2.transform.position.y - 18, 2);
				lr2.SetPosition(0, pos);
				lr2.SetPosition(1, pos2);
				moveDirection2 = Vector2.down;
				moveDirection2 = transform.TransformDirection(moveDirection2);
				moveDirection2 *= wallspeed;
				controller2.Move(moveDirection2 * Time.deltaTime);
			}
		}
		else if (walldirection == 6)
		{
			if (!controller2.isGrounded)
			{
				pos = new Vector3(p2.transform.position.x - 0.26f, p2.transform.position.y - 0.35f, 2);
				pos2 = new Vector3(p2.transform.position.x - 17.7f, p2.transform.position.y - 18, 2);
				lr2.SetPosition(0, pos);
				lr2.SetPosition(1, pos2);
				moveDirection2 = new Vector2(-1, -1);
				moveDirection2 = p2.transform.TransformDirection(moveDirection2);
				moveDirection2 *= wallspeed;
				if (hitname2 == "leftside") moveDirection2 = Vector3.zero;
				controller2.Move(moveDirection2 * Time.deltaTime);
			}
		}
		else if (walldirection == 7)
		{
			pos = new Vector3(p2.transform.position.x - 0.3f, p2.transform.position.y, 2);
			pos2 = new Vector3(p2.transform.position.x - 18, p2.transform.position.y, 2);
			lr2.SetPosition(0, pos);
			lr2.SetPosition(1, pos2);
			moveDirection2 = Vector2.left;
			moveDirection2 = p2.transform.TransformDirection(moveDirection2);
			moveDirection2 *= wallspeed;
			controller2.Move(moveDirection2 * Time.deltaTime);
		}
		else if (walldirection == 8)
		{
			pos = new Vector3(p2.transform.position.x - 0.2f, p2.transform.position.y + 0.3f, 2);
			pos2 = new Vector3(p2.transform.position.x - 18, p2.transform.position.y + 18, 2);
			lr2.SetPosition(0, pos);
			lr2.SetPosition(1, pos2);
			moveDirection2 = new Vector2(-1, 1);
			moveDirection2 = p2.transform.TransformDirection(moveDirection2);
			moveDirection2 *= wallspeed;
			if (hitname2 == "sky" || hitname2 == "leftside") moveDirection2 = Vector3.zero;
			controller2.Move(moveDirection2 * Time.deltaTime);
		}
	}

	public void slash2()
	{
		float i, j,abs,absi,absj;
		lr2.enabled = false;
		i = p2.transform.position.x - p2.transform.position.x;
		j = p2.transform.position.y - p2.transform.position.y;
		if (Mathf.Abs(i) < 1 && Mathf.Abs(j) < 0.81)
		{
			if(!(GameManager.Instance.LastAttackPlayer == p2.tag && attacking2) && ((i < 0 && !blockl2) || (i > 0 && !blockr2) || (i == 0)))
				Win("Blue");
			ci2 = 0;
			skystatus2 = 'c';
		}
		abs = Mathf.Abs(i) > Mathf.Abs(j) ? Mathf.Abs(i) : Mathf.Abs(j);
		absi = i / abs;
		absj = j / abs;
		if (absi <= 0) rslash2();
		else lslash2();
		moveDirection2 = new Vector2(absi, absj);
		moveDirection2 *= slashspeed;
		controller2.Move(-1 * moveDirection2 * Time.deltaTime);
	}

	public void close2()
	{
		slashing2 = true;
		if (ci2 > 7)
		{
			ci2 = -1;
			skystatus2 = 'g';
			moveDirection2 = new Vector2(0, 0);
			attacking2 = false;
		}
		controller2.Move(-1 * moveDirection2 * Time.deltaTime);
		ci2++;
	}

	public void lrunable2()
	{
		lr2.enabled = false;
	}

	void Win(string str)
	{
		GameManager.Instance.Gaming = false;
		GameManager.Instance.InGame.SetActive(true);
		GameManager.Instance.InGameText.GetComponent<Text>().text = str + " Win!!";
	}
}
