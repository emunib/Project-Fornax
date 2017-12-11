package njh;

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
			GameList.remove(game.Id);
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
	public CreateUserResponse CreateUser(UserLogin userLogin) {
		System.out.println("CreateUser was invoked... ");
		CreateUserResponse response = new CreateUserResponse();
		if (userLogin.getUsername().matches("^guest.*")){
			return CreateGuest();
		} else {
			PlayerImpl temp = UserList.get(userLogin.getUsername());
			if (temp == null){
				PlayerImpl newuser = new PlayerImpl(userLogin.getUsername(), userLogin.getPassword());
				UserList.put(newuser.getUsername(), newuser);
				SessionInfo session = newuser.LoginUser(userLogin).getSessionInfo();
				response.setSessionInfo(session);
				response.setResponse(CreateUserCode.SUCCESS);
			} else {
				response.setResponse(CreateUserCode.ALREADYEXISTS);
			}
		}

		System.out.println("...CreateUser is returning");
		return response;
	}

	public CreateUserResponse CreateGuest() {
		CreateUserResponse response = new CreateUserResponse();
		GuestImpl newuser = GuestFactory.GetNewGuest(this);
		UserList.put(newuser.getUsername(), newuser);
		UserLogin userLogin = new UserLogin();
		userLogin.setUsername(newuser.getUsername());
		userLogin.setPassword(newuser.Password);
		SessionInfo session = newuser.LoginUser(userLogin).getSessionInfo();
		response.setSessionInfo(session);
		response.setResponse(CreateUserCode.SUCCESS);
		System.out.println("...CreateUser is returning");
		return response;
	}

	@Override
	public PlayerImpl GetUser(String userID) {
		return UserList.get(userID);
	}

	public void DeleteUser(String username){
		UserList.remove(username);
	}

}
