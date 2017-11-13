package njh;
import Online.*;
import com.zeroc.Ice.Current;

import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

public class GameRegisterImpl implements GameRegister
{
    @Override
    public GamePrx[] GetGames(Current current) {
        return new GamePrx[0];
    }

    @Override
    public GameHostPrx CreateGame(ServerPrx server, Current current) {
        return null;
    }

    @Override
    public PlayerPrx Login(String password, String username, Current current) {
        return null;
    }
}
