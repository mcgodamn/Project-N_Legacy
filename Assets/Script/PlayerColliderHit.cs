using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColliderHit : MonoBehaviour {
	public string hitname;
	public int player;
	public SoloMode Script;
	void Start()
	{
		if (tag == "1p")
			player = 1;
		else if(tag == "2p")
			player = 2;
		Script = GameManager.Instance.GetComponent<SoloMode> ();
	}
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hitname != hit.gameObject.name)
		{
			if (player == 1) 
			{
				Script.walldirection = 0;
				Script.lrunable ();
				hitname = hit.gameObject.name;
				Script.hitname = hitname;
			} else if (player == 2) 
			{
				Script.walldirection2 = 0;
				Script.lrunable2 ();
				hitname = hit.gameObject.name;
				Script.hitname2 = hitname;
			}
		}
	}
}
