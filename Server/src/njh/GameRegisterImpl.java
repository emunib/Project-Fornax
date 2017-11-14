package njh;
import Online.*;
import com.zeroc.Ice.Current;
import com.zeroc.Ice.ObjectAdapter;
import com.zeroc.Ice.ObjectPrx;

import java.util.*;

public class GameRegisterImpl implements GameRegister, Runnable
{
    HashMap<String, User> UserList;
    HashMap<User, LobbyListenerPrx> ActiveUsers;
    private HashSet<GamePrx> GameList;
    boolean clean;
    boolean waiting;
    ObjectAdapter adapter;

    public GameRegisterImpl(ObjectAdapter nadapter){
        UserList = new HashMap<>();
        ActiveUsers = new HashMap<>();
        GameList = new HashSet<>();
        clean = true;
        waiting = false;
        adapter = nadapter;
       Thread updater = new Thread(this);
       updater.start();
    }


    public void run() {
        while (true){
            HashSet<GameImpl> GameListCopy;
            synchronized (GameList){
                GameListCopy = (HashSet) GameList.clone();
                clean = true;
            }
            synchronized (ActiveUsers){
                ActiveUsers.forEach((user, lobbyListenerPrx) -> {
                    lobbyListenerPrx.Update((GamePrx[])GameListCopy.toArray());
                });
            }
            try {
                synchronized (GameList){
                    if (clean){
                        waiting = true;
                        GameList.wait();
                    }
                }
                Thread.sleep(1000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
        }
    }

    @Override
    public PlayerPrx Login(String username, String password, LobbyListenerPrx lstnr, Current current) {
        User user = UserList.get(username);
        if (user == null){
            return null;
        } else if (ActiveUsers.get(user) != null) {
            return null;
        }else if (user.Password != password){
            return null;
        } else {
            PlayerImpl player = new PlayerImpl(user, adapter, this);
            PlayerPrx result = PlayerPrx.checkedCast(adapter.addWithUUID(player));
            ActiveUsers.put(user, lstnr);
            return result;
        }
    }

    @Override
    public PlayerPrx CreateNew(String username, String password, LobbyListenerPrx lstnr, Current current) {
        User temp = UserList.get(username);
        if (temp == null){
            User newuser = new User();
            newuser.Password = password;
            newuser.Username = username;
            UserList.put(username, newuser);
            ActiveUsers.put(newuser, lstnr);
            PlayerImpl player = new PlayerImpl(newuser, adapter, this);
            return PlayerPrx.checkedCast(adapter.addWithUUID(player));
        } else {
            return null;
        }
    }

    public GamePrx AddGame(GameImpl game){
        GamePrx newproxy;
        synchronized (GameList){
            if (clean == true){
                clean = false;
            }
            newproxy = GamePrx.checkedCast(adapter.addWithUUID(game));
            GameList.add(newproxy);
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
        return newproxy;
    }

    public void RemoveGame(GamePrx prx){
        synchronized (GameList) {
            if (clean == true){
                clean = false;
            }
            GameList.remove(prx);
            adapter.remove(prx.ice_getIdentity());
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
    }


}
