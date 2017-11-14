package njh;
import Online.*;
import com.zeroc.Ice.Current;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Objects;

import com.zeroc.Ice.*;

public class GameImpl implements Game {
	ObjectAdapter adapter;
	HashMap<Player, ClientPrx> PlayerList;
	protected Player Host;
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
		PlayerList.remove(player);
		return true;
	}
}
