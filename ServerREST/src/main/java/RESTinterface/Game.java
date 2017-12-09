package RESTinterface;

import XMLechangeable.*;
import com.sun.tools.internal.jxc.ap.Const;
import jdk.internal.util.xml.impl.Input;
import njh.InputState;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import java.util.List;

import static RESTinterface.Constants.*;

@Produces({ "application/xml"})
public interface Game {
	@POST
	@Path(UserPath) // The users in the game (the game lobby)
	PublicGameInfo JoinGame(SessionInfo sessionInfo);

	@GET
	@Path(LobbyInfoPath)// Returns all users in the game
	PublicGameInfo GetUsers();

	@DELETE
	@Path(UserPath + UserIdPath) // PlayerImpl leaves the game
	Response RemoveUser(@PathParam(UserId) String userID, @HeaderParam(PrivateID) String privateID);

	@GET  //Get the input of all agents in the game
	@Path(InputPath)
	List<InputState> GetInputs();

	@PUT // Updates the users input state
	@Path(InputPath + UserIdPath)
	Response UpdateInput(@PathParam(UserId) String userID, @HeaderParam(PrivateID) String privateID, InputState inputState);

	@GET // Returns all game objects
	@Path(GameobjectsPath)
	List<GameObject> GetGameObjects();

	@PUT // Updates all game objects
	@Path(GameobjectsPath)
	Response UpdateGameObjects(List<GameObject> gameObjectList);

	// /update - The current state of the game
	@POST // Creates a new update the current state (update)
	@Path(UpdatePath)
	Response CreateUpdate();

	/* @GET // Returns info on existing updates
	@Path("/update")
	UpdateInfo GetUpdateInfo(); */

	// /update/{updateID} - A particular update

	@GET // Returns the update
	@Path(UpdatePath + UpdateIdPath)
	Update GetUpdate(@PathParam(UpdateId) String updateID);
}
