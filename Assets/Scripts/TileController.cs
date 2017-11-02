using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : C_WorldObjectController {
	// Use this for initialization
	void Start () {
		Manager.ObjectLog.Add (gameObject, this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
