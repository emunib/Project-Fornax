package njh;

import RESTinterface.Game;
import RESTinterface.Player;
import RESTinterface.Register;
import XMLechangeable.*;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;



public class RegisterImpl implements Register {
	private final HashMap<String, PlayerImpl> UserList;
	private final HashMap<String, GameImpl> GameList;

	public RegisterImpl(){
		UserList = new HashMap<>();
		GameList = new HashMap<>();
	}

	public void RemoveGame(GameImpl game){
		synchronized (GameList) {
			GameList.remove(game);
			GameFactory.DisposeGame(game);
		}
	}

	@Override
	public PublicGameInfoList GetGames() {
		PublicGameInfoList publicGameInfoList = new PublicGameInfoList();
		List<PublicGameInfo> result = new LinkedList<>();
		GameList.forEach((s, game) -> {
			result.add(game.GetLobbyInfo());
		});
		publicGameInfoList.setPublicGameInfos(result);
		return publicGameInfoList;
	}

	@Override
	public PublicGameInfo CreateGame(SessionInfo session) {
		PublicGameInfo publicGameInfo = new PublicGameInfo();
		PlayerImpl player = UserList.get(session.getUsername());
		if (player == null) return publicGameInfo;
		if (player.isValidSession(session)){
			GameImpl game = GameFactory.CreateGame(player.getSession(session), this);
			publicGameInfo = game.GetLobbyInfo();
			GameList.put(game.Id, game);
		}
		return publicGameInfo;
	}

	@Override
	public GameImpl GetGame(String gameID) {
		return GameList.get(gameID);
	}

	@Override
	public SessionInfo CreateUser(UserLogin userLogin) {
		System.out.println("CreateUser was invoked... ");
		PlayerImpl temp = UserList.get(userLogin.getUsername());
		SessionInfo session = new SessionInfo();
		if (temp == null){
			PlayerImpl newuser = new PlayerImpl(userLogin.getUsername(), userLogin.getPassword());
			UserList.put(newuser.getUsername(), newuser);
			session = newuser.LoginUser(userLogin);
		}
		System.out.println("...CreateUser is returning");
		return session;
	}

	@Override
	public PlayerImpl GetUser(String userID) {
		return UserList.get(userID);
	}

}
