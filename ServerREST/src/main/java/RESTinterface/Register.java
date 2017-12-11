package RESTinterface;

import XMLechangeable.*;

import javax.ws.rs.*;

import java.util.List;

@Produces({ "application/xml"})
public interface Register {
	@POST
	@Path("/users") // Create a new user
	CreateUserResponse CreateUser(UserLogin userLogin);

	@Path("/users/{userID}") // A single user
	Player GetUser(@PathParam("userID") String userID);

	@GET // - returns a list of all games
	@Path("/games") // A representation of all games
	PublicGameInfoList GetGames();

	@POST
	@Path("/games") // A representation of all games
	PublicGameInfo CreateGame(SessionInfo session);

	@Path("/games/{gameID}")
	Game GetGame(@PathParam("gameID") String gameID);
}
