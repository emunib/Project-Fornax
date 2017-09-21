using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : C_WorldObject {
	public GameObject Player;
	public Rigidbody2D PlayerBody;
	public GrapplingHook(GameObject player){
		Player = player;
		PlayerBody = player.GetComponent<Rigidbody2D> ();
	}
}
