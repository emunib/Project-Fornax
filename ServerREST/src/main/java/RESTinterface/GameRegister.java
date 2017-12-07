package RESTinterface;

import RESTinterface.Game;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import java.util.List;

@Produces({ "application/xml"})
public interface GameRegister {
	@GET // - returns a list of all games
	@Path("/games") // A representation of all games
	List<Game> GetGames();

	@POST
	@Path("/games") // A representation of all games
	Response CreateGame();

	@Path("/games/{gameID}")
	Game GetGame(@PathParam("gameID") String gameID);

}