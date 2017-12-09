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
	private final HashMap <String, SessionImpl> sessionHashMap;
	private GameImpl activeGame;

	public PlayerImpl(String username, String password){
		setUsername(username);
		Password = password;
		sessionHashMap = new HashMap<>();
	}

	public boolean isEqualPassword(String password){
		return password.equals(Password);
	}

	public boolean isValidSession(SessionInfo sessionInfo){
		SessionImpl session = sessionHashMap.get(sessionInfo.getPublicID());
		if (session == null) return false;
		return session.isValidSession(sessionInfo);
	}

	public SessionImpl getSession(SessionInfo sessionInfo){
		return sessionHashMap.get(sessionInfo.getPublicID());
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
	public Response Logout(String sessionID, String PrivateID) {
		Response response;
		System.out.println("Logout was invoked... (sessionID = " + sessionID + " )");
		SessionImpl session = sessionHashMap.get(sessionID);
		if (session != null) {
			if (session.isValidSession(PrivateID)) {
				sessionHashMap.remove(sessionID);
				response = Response.ok().build();
			} else {
				response = Response.status(Response.Status.UNAUTHORIZED).build();
			}
		} else {
			response = Response.status(Response.Status.NOT_FOUND).build();
		}
		System.out.println("...Logout is returning");
		return response;
	}

	@Override
	public SessionInfo GetSession(String sessionID) {
		return null;
	}

	@Override
	public SessionInfo LoginUser(UserLogin userLogin) {
		SessionInfo newsessioninfo = new SessionInfo();
		if (isEqualPassword(userLogin.getPassword())){
			SessionImpl newSession = SessionFactory.GetNewSession(this);
			sessionHashMap.put(newSession.getPublicID(), newSession);
			newsessioninfo = newSession;
		}
		return newsessioninfo;
	}
}
