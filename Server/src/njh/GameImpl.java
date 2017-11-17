package njh;
import Online.*;
import generic.KeyPair;
import com.zeroc.Ice.Current;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Objects;

import com.zeroc.Ice.*;

class GameInfo {
	HashMap<PlayerImpl, ClientPrx> PlayerList;
	KeyPair<PlayerImpl, ServerPrx> Host;
	boolean Modified;

	public GameInfo(){
		PlayerList = new HashMap<>();
	}

	public synchronized boolean GetModified(){
		return Modified;
	}

	public synchronized HashMap<PlayerImpl, ClientPrx> GetPlayerList(){

	}

	public synchronized void SetHost(PlayerImpl player, ServerPrx server){
		Modified = true;
		Host = new KeyPair<>(player, server);
	}

	public synchronized KeyPair<PlayerImpl, ServerPrx> GetHost(){
		return Host;
	}

	public synchronized void AddPlayer(PlayerImpl player, ClientPrx clientPrx){
		Modified = true;
		PlayerList.put(player, clientPrx);
	}

	public synchronized void RemovePlayer(PlayerImpl player){
		Modified = true;
		PlayerList.remove(player);
	}
}

public class GameImpl implements Game, GameHost {
	ObjectAdapter adapter;

	GameRegisterImpl Register;

	boolean Joinable;
	LobbyInfo CacheLobbyInfo;


	public GameImpl(ServerPrx server, PlayerImpl player, GameRegisterImpl register) {
		HostServer = server;
		Host = player;
		Register = register;
		PlayerList = new HashMap<>();
		Joinable = true;
	}

	@Override
	public synchronized LobbyInfo GetLobbyInfo(Current current) {
		LobbyInfo result = new LobbyInfo();
		result.Players = new PlayerStats[PlayerList.size()];
		Counter i = new Counter();
		PlayerList.forEach((player, clientPrx) -> {
			result.Players[i.IncUp()] = player.GetStats(null);
		});
		result.Host = Host.GetStats(null);
		return result;
	}

	public synchronized boolean AddPlayer(PlayerImpl player, ClientPrx client){
		if (Joinable){
			PlayerList.put(player, client);

			return true;
		}
		return false;
	}

	public synchronized boolean RemovePlayer(PlayerImpl player){
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
	public synchronized void StartGame(Current current) {

	}

	@Override
	public synchronized void KickPlayer(String username, Current current) {

	}

	@Override
	public synchronized void LockRoom(Current current) {

	}

	@Override
	public synchronized void UnlockRoom(Current current) {

	}
}
