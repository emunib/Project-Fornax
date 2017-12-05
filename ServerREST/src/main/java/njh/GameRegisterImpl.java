package njh;
import Online.*;
import com.zeroc.Ice.*;

import java.lang.Exception;
import java.util.*;
import java.util.concurrent.CompletableFuture;

public class GameRegisterImpl implements GameRegister
{
    final public HashMap<LobbyListenerPrx, PlayerPrx> ActiveUsers;

    final private HashMap<GameImpl, GamePrx> GameList;
    final private HashMap<GameImpl, GamePrx> HiddenGameList;

    final private PlayerRegistry playerRegistry;
    private boolean clean;
    private boolean waiting;
    public final IdGenerator idGenerator;
    Communicator communicator;
    ObjectAdapter adapter;


    public GameRegisterImpl(ObjectAdapter nadapter, Communicator nCommunicator){
        playerRegistry = new PlayerRegistry(this, nadapter);
        ActiveUsers = new HashMap<>();
        GameList = new HashMap<>();
        HiddenGameList = new HashMap<>();
        clean = true;
        waiting = false;
        adapter = nadapter;
        idGenerator = new IdGenerator();
        communicator = nCommunicator;
       Thread updater = new Thread(new Updater());
       Thread pinger = new Thread(new Pinger());
       updater.start();
       pinger.start();
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

    public void HideGame(GameImpl game){
        synchronized (GameList){
            try {
                if (clean) {
                    clean = false;
                }
                GamePrx gamePrx = GameList.remove(game);
                if (gamePrx == null) throw new Exception();
                HiddenGameList.put(game, gamePrx);
                if (waiting) {
                    GameList.notify();
                    waiting = false;
                }
            } catch (Exception e){
                e.printStackTrace();
            }
        }
    }

    public void UnHideGame(GameImpl game){
        synchronized (GameList){
            try {
                if (clean) {
                    clean = false;
                }
                GamePrx gamePrx = HiddenGameList.remove(game);
                if (gamePrx == null) throw new Exception();
                GameList.put(game, gamePrx);
                if (waiting) {
                    GameList.notify();
                    waiting = false;
                }
            } catch (Exception e){
                e.printStackTrace();
            }
        }
    }

    public void RemoveGame(GameImpl game){
        synchronized (GameList) {
            if (clean){
                clean = false;
            }
            GamePrx proxy = GameList.remove(game);
            if (proxy == null){
                proxy = HiddenGameList.remove(game);
            }
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
