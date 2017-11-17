package njh;
import Online.*;
import generic.KeyPair;
import com.zeroc.Ice.Current;

import java.util.HashMap;
import java.util.concurrent.CompletableFuture;

import com.zeroc.Ice.*;
import generic.ReadOnlyHashMap;
import generic.ReadOnlyKeyPair;

class LocalLobbyInfo {
	public HashMap<PlayerImpl, ClientPrx> PlayerList;
	public KeyPair<PlayerImpl, ServerPrx> Host;
}

class GameInfo {
	private final HashMap<PlayerImpl, ClientPrx> PlayerList = new HashMap<>();
	private final KeyPair<PlayerImpl, ServerPrx> Host = new KeyPair<>(null, null);
	public final ReadOnlyKeyPair<PlayerImpl, ServerPrx> host = new ReadOnlyKeyPair<>(Host);
	public final ReadOnlyHashMap<PlayerImpl, ClientPrx> playerList = new ReadOnlyHashMap<>(PlayerList);
	private final LobbyInfo CachedLobbyInfo;
	private boolean Modified;

	public GameInfo(String id){
		CachedLobbyInfo = new LobbyInfo();
		CachedLobbyInfo.Id = id;
	}

	public synchronized LobbyInfo GetLobbyInfo(){
		if (Modified) {
			Modified = false;
			CachedLobbyInfo.Players = new PlayerStats[PlayerList.size()];
			Counter i = new Counter();
			PlayerList.forEach((player, clientPrx) -> {
				CachedLobbyInfo.Players[i.IncUp()] = player.GetStats(null);
			});
			CachedLobbyInfo.Host = Host.Key.GetStats(null);
		}
		return CachedLobbyInfo;
	}

	public synchronized void SetHost(PlayerImpl player, ServerPrx server){
		Modified = true;
		Host.Key = player;
		Host.Value = server;
	}


	public synchronized void AddPlayer(PlayerImpl player, ClientPrx clientPrx){
		Modified = true;
		PlayerList.put(player, clientPrx);
	}

	public synchronized ClientPrx RemovePlayer(PlayerImpl player){
		Modified = true;
		return PlayerList.remove(player);
	}
}

public class GameImpl implements Game, GameHost {
	private final GameInfo gameInfo;
	private final GameRegisterImpl Register;
	private final PlayerRegistry playerRegistry;
	private boolean Joinable;


	public GameImpl(ServerPrx server, PlayerImpl player, GameRegisterImpl register, PlayerRegistry nPlayerRegistry, String id) {
		gameInfo = new GameInfo(id);
		gameInfo.SetHost(player, server);
		Register = register;
		Joinable = true;
		playerRegistry = nPlayerRegistry;
	}

	@Override
	public synchronized LobbyInfo GetLobbyInfo(Current current) {
		return gameInfo.GetLobbyInfo();
	}

	public synchronized boolean AddPlayer(PlayerImpl player, ClientPrx client){
		if (Joinable){
			gameInfo.AddPlayer(player, client);
			return true;
		}
		return false;
	}

	public synchronized boolean RemovePlayer(PlayerImpl player){
		if (player == gameInfo.host.getKey()){
			if (gameInfo.playerList.isEmpty()){
				Register.RemoveGame(this);
			} else {
				//TODO migrate host
			}
		} else {
			gameInfo.RemovePlayer(player);
		}
		return true;
	}


	@Override
	public synchronized void StartGame(Current current) {
		Joinable = false;
	}

	@Override
	public synchronized void KickPlayer(String username, Current current) {
		PlayerImpl player = playerRegistry.FindPlayerImplByString(username);
		if (player != null){
			ClientPrx clientPrx = gameInfo.RemovePlayer(player);
			// Asynchronously notify player it has been kicked;
			clientPrx.NotifyKickedAsync();
		}

	}

	@Override
	public synchronized void LockRoom(Current current) {
		Joinable = false;
		Register.RemoveGame(this);
	}

	@Override
	public synchronized void UnlockRoom(Current current) {
		Joinable = true;
		Register.AddGame(this);
	}
}
