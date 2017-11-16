package njh;
import Online.*;
import com.zeroc.Ice.*;

import java.util.*;



public class GameRegisterImpl implements GameRegister
{
    HashMap<String, User> UserList;
    HashMap<User, LobbyListenerPrx> ActiveUsers;
    private HashMap<GameImpl, GamePrx> GameList;
    boolean clean;
    boolean waiting;
    ObjectAdapter adapter;

    public GameRegisterImpl(ObjectAdapter nadapter){
        UserList = new HashMap<>();
        ActiveUsers = new HashMap<>();
        GameList = new HashMap<>();
        clean = true;
        waiting = false;
        adapter = nadapter;
       Thread updater = new Thread(new Updater());
       Thread pinger = new Thread(new Pinger());
       updater.start();
       pinger.start();
    }

    class Updater implements Runnable {
        public void run() {
            while (true) {
                HashMap<GameImpl, GamePrx> GameListCopy;
                GamePrx[] activeGames;
                synchronized (GameList) {
                    GameListCopy = (HashMap) GameList.clone();
                    clean = true;
                }
                activeGames = new GamePrx[GameListCopy.size()];
                Counter counter = new Counter();
                GameListCopy.forEach((game, prx) -> {
                    activeGames[counter.IncUp()] = prx;
                });
                synchronized (ActiveUsers) {
                    ActiveUsers.forEach((user, lobbyListenerPrx) -> {
                        lobbyListenerPrx.Update(activeGames);
                    });
                }
                try {
                    synchronized (GameList) {
                        if (clean) {
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
    }

    class Pinger implements Runnable {
        @Override
        public void run() {
            while (true){
                HashSet<User> UnactiveUsers = new HashSet<>();
                synchronized (ActiveUsers) {
                    ActiveUsers.forEach((user, lobbyListenerPrx) -> {
                        try {
                            if (!lobbyListenerPrx.Ping()) {
                                UnactiveUsers.add(user);
                            }
                        } catch (ObjectNotExistException | ConnectionRefusedException e){
                            UnactiveUsers.add(user);
                        }
                    });
                    UnactiveUsers.forEach(user -> {
                        ActiveUsers.remove(user);
                    });
                }
                try {
                    Thread.sleep(5000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
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
        }else if (!user.Password.equals(password)){
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
            GameList.put(game, newproxy);
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
        return newproxy;
    }

    public void RemoveGame(GameImpl game){
        synchronized (GameList) {
            if (clean == true){
                clean = false;
            }
            GamePrx proxy = GameList.remove(game);
            adapter.remove(proxy.ice_getIdentity());
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
    }


}
