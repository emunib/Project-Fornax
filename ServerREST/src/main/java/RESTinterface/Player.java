package RESTinterface;

import XMLechangeable.*;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;

import static RESTinterface.Constants.*;

@Produces({ "application/xml"})
public interface Player {
	@GET
	@Path("/profile") // Gets the users profile
	UserPublic GetProfile();

	@PUT // Modifies some aspect of the users profile
	@Path("/profile") // A single user
	Response UpdateUser();

	@DELETE // Logs the user out
	@Path("/sessions/{sessionID}")
	Response Logout(@PathParam("sessionID") String sessionID, @HeaderParam(PrivateID) String privateID);

	@GET // Gets the sessions info
	@Path("/sessions/{sessionID}")
	SessionInfo GetSession(@PathParam("sessionID") String sessionID);

	@POST
	@Path("/sessions/") // A single user
	SessionInfo LoginUser(UserLogin userLogin);
}