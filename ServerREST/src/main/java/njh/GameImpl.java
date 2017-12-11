package njh;
import RESTinterface.Game;
import XMLechangeable.*;

import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.List;

import javax.ws.rs.core.Response;

import static sun.audio.AudioPlayer.player;

public class GameImpl implements Game {
	private final RegisterImpl Register;
	final String Id;

	private SessionImpl Host;
	private final PublicGameInfo CachedLobbyInfo;
	private final HashMap<String, SessionImpl> PlayerMap;
	private final LinkedList<SessionImpl> PlayerList;
	private boolean Locked;
	private boolean Joinable;
	private boolean Hidden;

	public boolean isHidden() {
		return Hidden;
	}

	public GameImpl(SessionImpl session, RegisterImpl register, String id) {
		PlayerMap = new HashMap<>();
		PlayerList = new LinkedList<>();
		CachedLobbyInfo = new PublicGameInfo();
		CachedLobbyInfo.setGameID(id);
		CachedLobbyInfo.setPlayers(new LinkedList<>());
		CachedLobbyInfo.setLocked(Locked);
		CachedLobbyInfo.getPlayers().add(session.player.GetProfile());
		SetHost(session);
		Id = id;
		PlayerMap.put(session.getUsername(), session);
		PlayerList.add(session);
		Register = register;
		Joinable = true;
		Locked = false;
	}


	public synchronized void SetHost(SessionImpl session){
		CachedLobbyInfo.setHost(session.player.GetProfile());
		Host = session;
	}



	public synchronized PublicGameInfo GetLobbyInfo() {
		return CachedLobbyInfo;
	}

	public synchronized boolean AddPlayer(PlayerImpl player){

		return false;
	}

	public synchronized boolean SwitchLock(){
		Locked = !Locked;
		CachedLobbyInfo.setLocked(Locked);
		Hidden = Locked;
		return Locked;
	}

	public synchronized void StartGame() {
		Hidden = true;
		Joinable = false;
	}

	@Override
	public synchronized PublicGameInfo JoinGame(SessionInfo sessionInfo) {
		SessionImpl session = Register.GetUser(sessionInfo.getUsername()).getSession(sessionInfo);
		PublicGameInfo publicGameInfo = new PublicGameInfo();
		if ((session != null) && (session.isValidSession(session))){
			if ((Joinable) && (!Locked)){
				if (!PlayerMap.containsValue(session)){
					CachedLobbyInfo.getPlayers().add(session.player.GetProfile());
					PlayerMap.put(session.getUsername(), session);
					PlayerList.add(session);
					publicGameInfo = GetLobbyInfo();
				}
			}
		}
		return publicGameInfo;
	}

	@Override
	public synchronized PublicGameInfo GetUsers() {
		return CachedLobbyInfo;
	}

	@Override
	public synchronized Response RemoveUser(String userID, String PrivateID) {
		Response response;
		SessionImpl session = PlayerMap.get(userID);
		if (session == null){
			response = Response.status(Response.Status.NOT_FOUND).build();
		}
		else {
			if ((session.isValidSession(PrivateID)) || (Host.isValidSession(PrivateID))){
				SessionImpl remove = PlayerMap.remove(userID);
				PlayerList.remove(remove);
				CachedLobbyInfo.getPlayers().remove(session.player.GetProfile());
				if (remove == Host){
					Host = null;
					if (PlayerMap.isEmpty()) {
						Register.RemoveGame(this);
					} else {
						SetHost(PlayerList.getFirst());
					}
				}
				response = Response.status(Response.Status.OK).build();
			} else {
				response = Response.status(Response.Status.UNAUTHORIZED).build();
			}
		}
		return response;
	}


	@Override
	public List<InputState> GetInputs() {
		return null;
	}

	@Override
	public Response UpdateInput(String userID, String privateID, InputState inputState) {
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
