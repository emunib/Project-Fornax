using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class LobbyListenerImpl
{
    public LinkedList<GamePrx> AvailableGames;
    public Mutex mutex;

    public LobbyListenerImpl(){
        mutex = new Mutex();
        AvailableGames = new LinkedList<GamePrx>();
    }

    public bool Ping()
    {
        return true;
    }

    public void Update(GamePrx[] list)
    {
        mutex.WaitOne();
        AvailableGames = new LinkedList<GamePrx>(list);
        mutex.ReleaseMutex();
    }
}
