using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Tile : C_WorldObject {

}

public class TileController : C_WorldObjectController {

	// Use this for initialization
	void Start () {
		SetObject (new Tile ());
		Manager.ObjectLog.Add (gameObject, this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
