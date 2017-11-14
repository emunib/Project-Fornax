package njh;
import Online.*;
import com.zeroc.Ice.Current;

public class GameHostImpl extends GameImpl implements GameHost {
	ServerPrx HostServer;

	public GameHostImpl(ServerPrx server, Player player){
		HostServer = server;
		Host = player;
	}
	@Override
	public void StartGame(Current current) {

	}
}
