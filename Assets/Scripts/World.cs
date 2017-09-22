using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public class Manager {
	public static Dictionary<GameObject, C_WorldObjectController> ObjectLog = new Dictionary<GameObject, C_WorldObjectController> ();
}


public class C_WorldObject {

}

public class C_WorldObjectController : MonoBehaviour {
	public C_WorldObject Object;
	public void SetObject(C_WorldObject nObject){
		Object = nObject; 
	}
}

public class World : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
