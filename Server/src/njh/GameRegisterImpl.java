package njh;
import Online.*;
import com.zeroc.Ice.*;

import java.util.*;

public class GameRegisterImpl implements GameRegister
{
    final public HashMap<LobbyListenerPrx, PlayerPrx> ActiveUsers;
    final private HashMap<GameImpl, GamePrx> GameList;
    final private PlayerRegistry playerRegistry;
    private boolean clean;
    private boolean waiting;
    public final IdGenerator idGenerator;
    ObjectAdapter adapter;


    public GameRegisterImpl(ObjectAdapter nadapter){
        playerRegistry = new PlayerRegistry(this, nadapter);
        ActiveUsers = new HashMap<>();
        GameList = new HashMap<>();
        clean = true;
        waiting = false;
        adapter = nadapter;
        idGenerator = new IdGenerator();
       Thread updater = new Thread(new Updater());
       Thread pinger = new Thread(new Pinger());
       updater.start();
       pinger.start();
    }

    @Override
    public PlayerRegisterPrx Connect(LobbyListenerPrx listener, Current current) {
        PlayerRegisterImpl playerRegister = new PlayerRegisterImpl(listener, this, playerRegistry);
        PlayerRegisterPrx playerRegisterPrx = PlayerRegisterPrx.checkedCast(adapter.addWithUUID(playerRegister));
        synchronized (GameList){
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
        synchronized (ActiveUsers){
            ActiveUsers.put(listener, null);
        }
        return playerRegisterPrx;
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
                HashSet<LobbyListenerPrx> UnactiveUsers = new HashSet<>();
                synchronized (ActiveUsers) {
                    ActiveUsers.forEach((lobbyListenerPrx, player) -> {
                            try {
                                lobbyListenerPrx.Update(activeGames);
                            } catch (ObjectNotExistException | ConnectTimeoutException e) {
                                UnactiveUsers.add(lobbyListenerPrx);
                            } catch (ConnectFailedException e){
                                e.printStackTrace();
                            }

                    });
                    flushInactiveUser(UnactiveUsers);
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
                HashSet<LobbyListenerPrx> UnactiveUsers = new HashSet<>();
                synchronized (ActiveUsers) {
                    ActiveUsers.forEach((lobbyListenerPrx, player) -> {
                        try {
                            if (!lobbyListenerPrx.Ping()) {
                                UnactiveUsers.add(lobbyListenerPrx);
                            }
                        } catch (ObjectNotExistException | ConnectionRefusedException | ConnectTimeoutException e){
                            UnactiveUsers.add(lobbyListenerPrx);
                        } catch (ConnectFailedException e){
                            e.printStackTrace();
                        }
                    });
                    flushInactiveUser(UnactiveUsers);
                }
                try {
                    Thread.sleep(5000);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }


    public GamePrx AddGame(GameImpl game){
        GamePrx newproxy;
        synchronized (GameList){
            if (clean){
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
            if (clean){
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

    private void flushInactiveUser(HashSet<LobbyListenerPrx> UnactiveUsers){
        UnactiveUsers.forEach(lobbyListenerPrx -> {
            PlayerPrx player = ActiveUsers.get(lobbyListenerPrx);
            if (player != null) {
                PlayerImpl temp = (PlayerImpl) adapter.findByProxy(player);
                if (temp != null) {
                    player.LogOut(null);
                }
            }
            ActiveUsers.remove(lobbyListenerPrx);
        });
    }
}
