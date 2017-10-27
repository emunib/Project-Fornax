using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : C_WorldObject {
	public C_Player Player;
	public PlayerController PlayerObject;
	public Rigidbody2D PlayerBody;
	public GrapplingHook(PlayerController player){
		Player = player.Object as C_Player;
		PlayerObject = player;
		PlayerBody = player.gameObject.GetComponent<Rigidbody2D> ();
	}
}
