using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading;
using System.Runtime.InteropServices;
using Demo;

public class PlayerManager {
	public static HashSet<C_PlayerController> PlayerLog = new HashSet<C_PlayerController> ();
	public static System.Threading.Mutex myMutex = new Mutex();

	public static int AddPlayer(C_PlayerController player){
		int result = -1;
		myMutex.WaitOne ();
		if (PlayerLog.Contains(player)){
			throw new Exception("Player already in log");
		} else {
			result = PlayerLog.Count;
			PlayerLog.Add(player);
		}
		myMutex.ReleaseMutex ();
        String[] array = { result.ToString() };
       
		return result;
	}
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

public class ListenerI : ListenerDisp_
{
    public override void PingLstnr(string word, Ice.Current current){
        Debug.Log(word);
    }
}
 
public class Client
{
    public static int Main(string[] args)
    {
        try
        {
            Ice.Communicator communicator = Ice.Util.initialize(ref args);
            Ice.ObjectAdapter adapater = communicator.createObjectAdapterWithEndpoints("Test", "tcp -h 127.0.0.1 -p 10001");
            Ice.ObjectPrx obj = communicator.stringToProxy("SimplePrinter:tcp -h 127.0.0.1 -p 10000");
            ServerPrx printer = ServerPrxHelper.checkedCast(obj);
            if (printer == null)
            {
                throw new ApplicationException("Invalid proxy");
            }
            adapater.activate();
            printer.register(ListenerPrxHelper.checkedCast(adapater.add(new ListenerI(), Ice.Util.stringToIdentity("Test"))));
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return 1;
        }
        return 0;
    }
}
