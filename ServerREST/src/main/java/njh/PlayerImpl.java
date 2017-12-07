package njh;

import RESTinterface.Player;
import XMLechangeable.*;

import javax.ws.rs.core.Response;
import java.util.HashMap;
import java.util.List;
import java.util.Random;
import java.util.TimeZone;

public class PlayerImpl extends UserPublic implements Player {
	private final String Password;
	private final HashMap <String, Session> sessionHashMap;

	public PlayerImpl(String username, String password){
		setUsername(username);
		Password = password;
		sessionHashMap = new HashMap<>();
	}

	public boolean isEqualPassword(String password){
		return password.equals(Password);
	}

	@Override
	public UserPublic GetProfile() {
		return (UserPublic)this;
	}

	@Override
	public Response UpdateUser() {
		return null;
	}

	@Override
	public Response Logout(String sessionID) {
		return null;
	}

	@Override
	public SessionInfo GetSession(String sessionID) {
		return null;
	}

	@Override
	public Session LoginUser(UserLogin userLogin) {
		Session newsession = new Session();
		if (isEqualPassword(userLogin.getPassword())){
			newsession = SessionFactory.GetNewSession(getUsername());
			sessionHashMap.put(newsession.getPublicID(), newsession);
		}
		return newsession;
	}
}
