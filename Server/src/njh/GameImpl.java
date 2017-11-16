package njh;
import Online.*;
import com.zeroc.Ice.Current;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Objects;

import com.zeroc.Ice.*;

public class GameImpl implements Game, GameHost {
	ServerPrx HostServer;
	ObjectAdapter adapter;
	HashMap<PlayerImpl, ClientPrx> PlayerList;
	protected PlayerImpl Host;
	GameRegisterImpl Register;


	public GameImpl(ServerPrx server, PlayerImpl player, GameRegisterImpl register) {
		HostServer = server;
		Host = player;
		Register = register;
	}

	@Override
	public PlayerStats[] GetPlayers(Current current) {
		PlayerStats[] result = new PlayerStats[PlayerList.size() + 1];
		Counter i = new Counter();
		PlayerList.forEach((player, clientPrx) -> {
			result[i.IncUp()] = player.GetStats(null);
		});
		result[i.IncUp()] = Host.GetStats(null);
		return result;
	}

	public boolean AddPlayer(PlayerImpl player, ClientPrx client){
		PlayerList.put(player, client);
		return true;
	}

	public boolean RemovePlayer(PlayerImpl player){
		if (player == Host){
			if (PlayerList.isEmpty()){
				Register.RemoveGame(this);
			} else {
				//TODO migrate host
			}
		} else {
			PlayerList.remove(player);
		}
		return true;
	}


	@Override
	public void StartGame(Current current) {

	}
}
