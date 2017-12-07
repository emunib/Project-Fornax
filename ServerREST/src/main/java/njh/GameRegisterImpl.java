package njh;
import RESTinterface.Game;
import RESTinterface.GameRegister;

import javax.ws.rs.core.Response;
import java.lang.Exception;
import java.util.*;

public class GameRegisterImpl implements GameRegister
{
    final private HashMap<GameImpl, String> GameList;
    final private HashMap<GameImpl, String> HiddenGameList;

    private boolean clean;
    private boolean waiting;
    public final IdGenerator idGenerator;


    public GameRegisterImpl(){
        GameList = new HashMap<>();
        HiddenGameList = new HashMap<>();
        clean = true;
        waiting = false;
        idGenerator = new IdGenerator();
    }


    public void AddGame(GameImpl game){
        synchronized (GameList){
            if (clean){
                clean = false;
            }
            /*newproxy = GamePrx.checkedCast(adapter.addWithUUID(game));
            GameList.put(game, newproxy); */
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
    }

    public void HideGame(GameImpl game){
        synchronized (GameList){
            try {
                if (clean) {
                    clean = false;
                }
                /*GamePrx gamePrx = GameList.remove(game);
                if (gamePrx == null) throw new Exception();
                HiddenGameList.put(game, gamePrx); */
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
                /*GamePrx gamePrx = HiddenGameList.remove(game);
                if (gamePrx == null) throw new Exception();
                GameList.put(game, gamePrx); */
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
            /*
            GamePrx proxy = GameList.remove(game);
            if (proxy == null){
                proxy = HiddenGameList.remove(game);
            } */
            //adapter.remove(proxy.ice_getIdentity());
            if (waiting){
                GameList.notify();
                waiting = false;
            }
        }
    }

    @Override
    public List<Game> GetGames() {
        return null;
    }

    @Override
    public Response CreateGame() {
        return null;
    }

    @Override
    public Game GetGame(String gameID) {
        return null;
    }
}
