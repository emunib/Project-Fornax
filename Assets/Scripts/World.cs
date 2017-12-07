using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;
using System.Runtime.InteropServices;
using Schemas;

public class PlayerManager {
	public static HashSet<C_PlayerController> PlayerLog = new HashSet<C_PlayerController> ();
	public static System.Threading.Mutex myMutex = new Mutex();

	public static int AddPlayer(C_PlayerController player){
		int result = -1;
		myMutex.WaitOne ();
		if (PlayerLog.Contains(player)){
			throw new System.Exception("Player already in log");
		} else {
			result = PlayerLog.Count;
			PlayerLog.Add(player);
		}
		myMutex.ReleaseMutex ();
        String[] array = { result.ToString() };
       
		return result;
	}
}

public class OnlineManager
{
	public static session Player;
    public static LobbyListenerImpl LobbyLstnrImpl;
    public static GamePrx Game;
}

public class Manager {
	public static Dictionary<GameObject, C_WorldObjectController> ObjectLog = new Dictionary<GameObject, C_WorldObjectController> ();
}



public class C_WorldObjectController : MonoBehaviour {
	
}

public class World : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

