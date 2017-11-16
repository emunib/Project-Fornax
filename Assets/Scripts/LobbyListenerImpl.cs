using UnityEngine;
using System.Collections;
using Online;
using Ice;
using System.Collections.Generic;
using System.Threading;

public class LobbyListenerImpl : LobbyListenerDisp_
{
    public LinkedList<GamePrx> AvailableGames;
    public Mutex mutex;

    public LobbyListenerImpl(){
        mutex = new Mutex();
        AvailableGames = new LinkedList<GamePrx>();
    }

    public override bool Ping(Current current = null)
    {
        return true;
    }

    public override void Update(GamePrx[] list, Current current = null)
    {
        mutex.WaitOne();
        AvailableGames = new LinkedList<GamePrx>(list);
        mutex.ReleaseMutex();
    }
}
