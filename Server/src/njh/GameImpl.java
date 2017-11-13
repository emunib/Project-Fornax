package njh;
import Online.*;
import com.zeroc.Ice.Current;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Objects;

import com.zeroc.Ice.*;

public class GameImpl implements Game {
	ObjectAdapter adapter;
	HashMap<PlayerPrx, ClientPrx> PlayerList;

	ServerPrx Host;
	@Override
	public PlayerStats[] GetPlayers(Current current) {
		adapter.findByProxy();

		return new PlayerStats[0];
	}
}
