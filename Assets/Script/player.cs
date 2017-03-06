using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour {
    public float wallspeed;
    public float slashspeed;
    public bool PlayerNum;
    public bool WalkReturn;
    public int Num;
    public Sprite[] PlayerSprite;
    public Sprite BasicSprite;
    public int ci;
    public bool slashing,attacking;
    public Vector2 moveDirection;
    CharacterController controller;
    LineRenderer lr;
    // Use this for initialization
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        controller = GetComponent<CharacterController>();
        PlayerSprite = Resources.LoadAll<Sprite>("");
		if (GameManager.Instance.player)
		{
			GetComponent<LineRenderer> ().startColor = Color.red;
			GetComponent<LineRenderer> ().endColor = Color.red; 
			PlayerNum = true;
			GameManager.Instance.player = !GameManager.Instance.player;
			Num = 1;
			tag = "1p";
		}
		else
		{
			GetComponent<LineRenderer> ().startColor = Color.blue;
			GetComponent<LineRenderer> ().endColor = Color.blue;
			PlayerNum = false;
			GameManager.Instance.player = !GameManager.Instance.player;
			Num = 2;
			tag = "2p";
		}
		GameManager.Instance.MyCharacter = gameObject;
    }
    public void Start ()
    {
		attacking = false;
        slashing = false;
        WalkReturn = true;
        if(PlayerNum)
        {
            transform.position = new Vector3(-4, -3.565f, 0);
            BasicSprite = PlayerSprite[11];
        }
        else
        {
            transform.position = new Vector3(4, -3.565f, 0);
            BasicSprite = PlayerSprite[7];
        }
        GetComponent<SpriteRenderer>().sprite = BasicSprite;
    }

    public void rslash()
    {
		Debug.Log ("rs");
        if (PlayerNum) GetComponent<SpriteRenderer>().sprite = PlayerSprite[14];
        else GetComponent<SpriteRenderer>().sprite = PlayerSprite[6];
        controller.radius = 0.53f;
        controller.height = 0.81f;
        controller.center = new Vector3(0.09f, 0.03f, 0);
        if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
        WalkReturn = false;
    }
    public void lslash()
	{
		Debug.Log ("Ls");
        if (PlayerNum) GetComponent<SpriteRenderer>().sprite = PlayerSprite[10];
        else GetComponent<SpriteRenderer>().sprite = PlayerSprite[3];
        controller.radius = 0.53f;
        controller.height = 0.81f;
        controller.center = new Vector3(0.09f, 0.03f, 0);
        if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
        WalkReturn = false;
    }

    public void right()
	{
		Debug.Log ("r");
        if(!slashing)
        {
            if (PlayerNum) GetComponent<SpriteRenderer>().sprite = PlayerSprite[13];
            else GetComponent<SpriteRenderer>().sprite = PlayerSprite[5];
        }
        controller.radius = 0.53f;
        controller.height = 0.81f;
        controller.center = new Vector3(0.09f, 0.03f, 0);
        if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
        WalkReturn = false;
    }

    public void left()
	{
		Debug.Log ("L");
        if(!slashing)
        {
            if (PlayerNum) GetComponent<SpriteRenderer>().sprite = PlayerSprite[9];
            else GetComponent<SpriteRenderer>().sprite = PlayerSprite[2];
        }
        controller.radius = 0.53f;
        controller.height = 0.81f;
        controller.center = new Vector3(0.09f, 0.03f, 0);
        if (WalkReturn) controller.Move(new Vector2(0, -0.56f));
        WalkReturn = false;
    }

    public void stand()
    {
		if (!GameManager.Instance.Gaming)
			return;
        GetComponent<SpriteRenderer>().sprite = BasicSprite;
        controller.radius = 0.38f;
        controller.height = 1.47f;
        controller.center = new Vector3(-0.09f, -0.12f, 0);
        if(controller.isGrounded) transform.position = new Vector3(transform.position .x, -3.565f,0);
        WalkReturn = true;
    }

    public void bright()
    {
        if (PlayerNum) GetComponent<SpriteRenderer>().sprite = PlayerSprite[12];
        else GetComponent<SpriteRenderer>().sprite = PlayerSprite[4];
    }

    public void bleft()
    {
        if (PlayerNum) GetComponent<SpriteRenderer>().sprite = PlayerSprite[8];
        else GetComponent<SpriteRenderer>().sprite = PlayerSprite[1];
    }

    public void wall(int walldirection,bool islocal)
    {
        Vector3 pos,pos2;
        lr.enabled = true;
        if(transform.position.x > -8.3f && transform.position.x < 8.1f && transform.position.y < 4.35f)
        {
            Debug.Log("hitno");
            GetComponent<CharController>().hitname = "";
        }
        if (walldirection == 3)
        {
            pos = new Vector3(transform.position.x + 0.44f, transform.position.y, 2);
            pos2 = new Vector3(transform.position.x + 18.3f, transform.position.y, 2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos2);
            moveDirection = Vector2.right;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= wallspeed;
            if(islocal) controller.Move(moveDirection * Time.deltaTime);
        }
        else if (walldirection == 1)
        {
            pos = new Vector3(transform.position.x, transform.position.y - 0.33f, 2);
            pos2 = new Vector3(transform.position.x, transform.position.y + 18, 2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos2);
            moveDirection = Vector2.up;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= wallspeed;
            if (islocal) controller.Move(moveDirection * Time.deltaTime);
        }
        else if (walldirection == 2)
        {
            pos = new Vector3(transform.position.x + 0.3f, transform.position.y + 0.28f, 2);
            pos2 = new Vector3(transform.position.x + 18.3f, transform.position.y + 18, 2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos2);
            moveDirection = Vector2.one;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= wallspeed;
            if (GetComponent<CharController>().hitname == "sky" || GetComponent<CharController>().hitname == "rightside") moveDirection = Vector3.zero;
            if (islocal) controller.Move(moveDirection * Time.deltaTime);
        }
        else if (walldirection == 4)
        {
            if (!controller.isGrounded)
            {
                pos = new Vector3(transform.position.x + 0.51f, transform.position.y - 0.42f, 2);
                pos2 = new Vector3(transform.position.x + 18.3f, transform.position.y - 18, 2);
                lr.SetPosition(0, pos);
                lr.SetPosition(1, pos2);
                moveDirection = new Vector2(1, -1);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= wallspeed;
                if (GetComponent<CharController>().hitname == "rightside") moveDirection = Vector3.zero;
                if (islocal) controller.Move(moveDirection * Time.deltaTime);
            }
        }
        else if (walldirection == 5)
        {
            if (!controller.isGrounded)
            {
                pos = new Vector3(transform.position.x, transform.position.y - 0.37f, 2);
                pos2 = new Vector3(transform.position.x, transform.position.y - 18, 2);
                lr.SetPosition(0, pos);
                lr.SetPosition(1, pos2);
                moveDirection = Vector2.down;
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= wallspeed;
                if (islocal) controller.Move(moveDirection * Time.deltaTime);
            }
        }
        else if (walldirection == 6)
        {
            if (!controller.isGrounded)
            {
                pos = new Vector3(transform.position.x - 0.26f, transform.position.y - 0.35f, 2);
                pos2 = new Vector3(transform.position.x - 17.7f, transform.position.y - 18, 2);
                lr.SetPosition(0, pos);
                lr.SetPosition(1, pos2);
                moveDirection = new Vector2(-1, -1);
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= wallspeed;
                if (GetComponent<CharController>().hitname == "leftside") moveDirection = Vector3.zero;
                if (islocal) controller.Move(moveDirection * Time.deltaTime);
            }
        }
        else if (walldirection == 7)
        {
            pos = new Vector3(transform.position.x - 0.3f, transform.position.y, 2);
            pos2 = new Vector3(transform.position.x - 18, transform.position.y, 2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos2);
            moveDirection = Vector2.left;
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= wallspeed;
            if (islocal) controller.Move(moveDirection * Time.deltaTime);
        }
        else if (walldirection == 8)
        {
            pos = new Vector3(transform.position.x - 0.2f, transform.position.y + 0.3f, 2);
            pos2 = new Vector3(transform.position.x - 18, transform.position.y + 18, 2);
            lr.SetPosition(0, pos);
            lr.SetPosition(1, pos2);
            moveDirection = new Vector2(-1, 1);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= wallspeed;
            if (GetComponent<CharController>().hitname == "sky" || GetComponent<CharController>().hitname == "leftside") moveDirection = Vector3.zero;
            if (islocal) controller.Move(moveDirection * Time.deltaTime);
        }
    }

    public void slash()
    {
        CharController b;
        GameObject player2;
        if(PlayerNum) player2 = GameObject.FindWithTag("2p");
        else player2 = GameObject.FindWithTag("1p");
        float i, j,abs,absi,absj;
        lr.enabled = false;
        i = transform.position.x - player2.transform.position.x;
        j = transform.position.y - player2.transform.position.y;
        if (Mathf.Abs(i) < 1 && Mathf.Abs(j) < 0.81)
        {
            b = player2.GetComponent<CharController>();
			if(!(GameManager.Instance.LastAttackPlayer == tag && player2.GetComponent<player>().attacking) && ((i < 0 && !b.blockl) || (i > 0 && !b.blockr) || (i == 0)))
				GetComponent<CharController>().Win();
            ci = 0;
            GetComponent<CharController>().skystatus = 'c';
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
            GameObject player2;
            if (PlayerNum) player2 = GameObject.FindWithTag("2p");
            else player2 = GameObject.FindWithTag("1p");
            GetComponent<CharController>().skystatus = 'g';
            moveDirection = new Vector2(0, 0);
			attacking = false;
        }
        //if (GetComponent<CharController>().hitname == "rightside" || GetComponent<CharController>().hitname == "leftside")
        //{
        //    Debug.Log("11111");
        //    moveDirection.x = 0;
        //    //moveDirection.y /= Mathf.Abs(moveDirection.y);
        //}
        //else if (GetComponent<CharController>().hitname == "sky")
        //{
        //    Debug.Log("11111s");
        //    moveDirection.y = 0;
        //    //moveDirection.x /= Mathf.Abs(moveDirection.x);
        //}
        //else if (transform.position.y < -3.9)
        //{
        //    moveDirection.y = 0;
        //    //moveDirection.x /= Mathf.Abs(moveDirection.x);
        //}
        controller.Move(-1 * moveDirection * Time.deltaTime);
        ci++;
    }

    public void lrunable()
    {
        lr.enabled = false;
    }
}
