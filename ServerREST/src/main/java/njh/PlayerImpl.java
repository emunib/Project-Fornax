package njh;

import RESTinterface.Player;
import XMLechangeable.*;

import javax.ws.rs.core.Response;
import java.util.HashMap;
import java.util.List;
import java.util.Random;
import java.util.TimeZone;

public class PlayerImpl extends UserPublic implements Player {
	protected final String Password;
	protected SessionImpl session;
	protected GameImpl activeGame;

	public PlayerImpl(String username, String password){
		setUsername(username);
		Password = password;
	}

	public boolean isEqualPassword(String password){
		return password.equals(Password);
	}

	public boolean isValidSession(SessionInfo sessionInfo){
		if (session == null) return false;
		return session.isValidSession(sessionInfo);
	}

	public SessionImpl getSession(SessionInfo sessionInfo){
		return session;
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
		if (session != null) {
			if (session.isValidSession(PrivateID)) {
				SessionFactory.DiposeSession(session);
				session = null;
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
	public LoginResponse LoginUser(UserLogin userLogin) {
		LoginResponse loginResponse = new LoginResponse();
		if (session != null){
			if (session.isActive()){
				loginResponse.setResponse(LoginCode.ALREADYACTIVE);
				return loginResponse;
			} else {
				Logout(session.getPrivateID(), session.getPublicID());
			}
		}

		if (isEqualPassword(userLogin.getPassword())){
			SessionImpl newSession = SessionFactory.GetNewSession(this);
			session = newSession;
			loginResponse.setResponse(LoginCode.SUCCESS);
			loginResponse.setSessionInfo(newSession);
		} else {
			loginResponse.setResponse(LoginCode.BADPASSWORD);
		}
		return loginResponse;
	}
}
