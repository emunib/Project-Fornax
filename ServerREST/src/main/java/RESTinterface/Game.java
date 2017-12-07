package RESTinterface;

import XMLechangeable.GameObject;
import XMLechangeable.Update;
import XMLechangeable.UserPublic;
import njh.InputState;

import javax.ws.rs.*;
import javax.ws.rs.core.Response;
import java.util.List;

@Produces({ "application/xml"})
public interface Game {
	@POST
	@Path("/users") // The users in the game (the game lobby)
	Response JoinGame();
	@GET
	@Path("/users")// Returns all users in the game
	List<UserPublic> GetUsers();

	@DELETE
	@Path("/users/{userID}") // PlayerImpl leaves the game
	Response RemoveUser();

	@GET  //Get the input of all agents in the game
	@Path("/input")
	List<InputState> GetInputs();

	@PUT // Updates the users input state
	@Path("/input/{userID}")
	Response UpdateInput(@PathParam("userID") String userID, InputState inputState);

	@GET // Returns all game objects
	@Path("/gameobjects")
	List<GameObject> GetGameObjects();

	@PUT // Updates all game objects
	@Path("/gameobjects")
	Response UpdateGameObjects(List<GameObject> gameObjectList);

	// /update - The current state of the game
	@POST // Creates a new update the current state (update)
	@Path("/update")
	Response CreateUpdate();

	/* @GET // Returns info on existing updates
	@Path("/update")
	UpdateInfo GetUpdateInfo(); */

	// /update/{updateID} - A particular update

	@GET // Returns the update
	@Path("/update/{updateID}")
	Update GetUpdate(@PathParam("updateID") String updateID);
}
