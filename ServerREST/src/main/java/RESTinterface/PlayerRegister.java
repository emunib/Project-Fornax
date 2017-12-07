package RESTinterface;

import XMLechangeable.UserLogin;
import XMLechangeable.Session;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;

@Produces({ "application/xml"})
public interface PlayerRegister {
	@POST
	//@Path("/users") // Create a new user
	Session CreateUser(UserLogin userLogin);

	@Path("/{userID}") // A single user
	Player GetUser(@PathParam("userID") String userID);
}
