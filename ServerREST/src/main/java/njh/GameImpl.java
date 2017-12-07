package njh;
import RESTinterface.Game;
import XMLechangeable.GameObject;
import XMLechangeable.Update;
import XMLechangeable.UserPublic;

import java.util.HashMap;
import java.util.List;

import javax.ws.rs.core.Response;

class LocalLobbyInfo {
	public HashMap<PlayerImpl, String> PlayerList;
	//public KeyPair<PlayerImpl, String> Host;
}

class GameInfo {
	private final HashMap<PlayerImpl, String> PlayerList = new HashMap<>();
	//private final KeyPair<PlayerImpl, String> Host = new KeyPair<>(null, null);
	//public final ReadOnlyKeyPair<PlayerImpl, String> host = new ReadOnlyKeyPair<>(Host);
	//public final ReadOnlyHashMap<PlayerImpl, String> playerList = new ReadOnlyHashMap<>(PlayerList);
	//private final LobbyInfo CachedLobbyInfo;
	private boolean Modified;
	private boolean IsLocked;

	public GameInfo(String id){
		//CachedLobbyInfo = new LobbyInfo();
		//CachedLobbyInfo.Id = id;
		IsLocked = false;
	}

	public synchronized void GetLobbyInfo(){
		if (Modified) {
			Modified = false;
			//CachedLobbyInfo.Players = new PlayerStats[PlayerList.size()];
			Counter i = new Counter();
			PlayerList.forEach((player, clientPrx) -> {
				//CachedLobbyInfo.Players[i.IncUp()] = player.GetStats(null);
			});
			//CachedLobbyInfo.Host = Host.Key.GetStats(null);
			//CachedLobbyInfo.IsLocked = IsLocked;
		}
		//return CachedLobbyInfo;
	}

	public synchronized void SetHost(PlayerImpl player){
		Modified = true;
		//Host.Key = player;
		//Host.Value = server;
	}


	public synchronized void AddPlayer(PlayerImpl player){
		Modified = true;
		//PlayerList.put(player, clientPrx);
	}

	public synchronized void RemovePlayer(PlayerImpl player){
		Modified = true;
		//return PlayerList.remove(player);
	}

	public synchronized boolean SwitchLock(){
		Modified = true;
		IsLocked = !IsLocked;
		return IsLocked;
	}
}

public class GameImpl implements Game {
	private final GameInfo gameInfo;
	private final GameRegisterImpl Register;
	private boolean Locked;
	private boolean Joinable;


	public GameImpl( PlayerImpl player, GameRegisterImpl register, String id) {
		gameInfo = new GameInfo(id);
		//gameInfo.SetHost(player, server);
		Register = register;
		Joinable = true;
		//playerRegistry = nPlayerRegistry;
		Locked = false;
	}

	public synchronized void GetLobbyInfo() {
		//return gameInfo.GetLobbyInfo();
	}

	public synchronized boolean AddPlayer(PlayerImpl player){
		if ((Joinable) && (!Locked)){
			//gameInfo.AddPlayer(player, client);
			return true;
		}
		return false;
	}

	public synchronized boolean RemovePlayer(PlayerImpl player){
		/*if (player == gameInfo.host.getKey()){
			if (gameInfo.playerList.isEmpty()){
				Register.RemoveGame(this);
			} else {
				//TODO migrate host
			}
		} else {
			gameInfo.RemovePlayer(player);
		} */
		return true;
	}


	public synchronized void StartGame() {
		Register.HideGame(this);
		Joinable = false;
	}

	public synchronized void KickPlayer(String username) {
		/* PlayerImpl player = playerRegistry.FindPlayerImplByString(username);
		if (player != null){
			ClientPrx clientPrx = gameInfo.RemovePlayer(player);
			// Asynchronously notify player it has been kicked;
			clientPrx.NotifyKickedAsync();
		} */

	}

	public synchronized void SwitchLock() {
		if ((Locked = gameInfo.SwitchLock())){
			Register.HideGame(this);
		} else {
			Register.UnHideGame(this);
		}

	}

	@Override
	public Response JoinGame() {
		return null;
	}

	@Override
	public List<UserPublic> GetUsers() {
		return null;
	}

	@Override
	public Response RemoveUser() {
		return null;
	}

	@Override
	public List<InputState> GetInputs() {
		return null;
	}

	@Override
	public Response UpdateInput(String userID, InputState inputState) {
		return null;
	}

	@Override
	public List<GameObject> GetGameObjects() {
		return null;
	}

	@Override
	public Response UpdateGameObjects(List<GameObject> gameObjectList) {
		return null;
	}

	@Override
	public Response CreateUpdate() {
		return null;
	}

	@Override
	public Update GetUpdate(String updateID) {
		return null;
	}
}
