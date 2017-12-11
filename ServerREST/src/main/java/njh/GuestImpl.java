package njh;

import RESTinterface.Player;
import XMLechangeable.*;

import javax.ws.rs.core.Response;
import java.util.HashMap;
import java.util.List;
import java.util.Random;
import java.util.TimeZone;

public class GuestImpl extends PlayerImpl {
	private final RegisterImpl register;
	public final String Password;

	public GuestImpl(RegisterImpl register, String username, String Password){
		super(username, Password);
		this.register = register;
		this.Password = Password;
	}

	@Override
	public Response Logout(String sessionID, String PrivateID) {
		Response response;
		System.out.println("Logout was invoked... (sessionID = " + sessionID + " )");

		if (session != null) {
			if (session.isValidSession(PrivateID)) {
				this.register.DeleteUser(getUsername());
				SessionFactory.DiposeSession(session);
				GuestFactory.DisposeGuest(this);
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
}
