package RESTinterface;

public interface Constants {
	public static final String UserPath = "/users";
	public static final String UserId = "userID";
	public static final String UserIdPath = "/{" + UserId + "}";

	String LobbyInfoPath = "/lobbyinfo";

	public static final String InputPath = "/input";

	public static final String GameobjectsPath = "/gameobjects";

	public static final String UpdatePath = "/update";
	public static final String UpdateId = "updateID";
	public static final String UpdateIdPath = "/{"+ UpdateId +"}";

	public static final String PrivateID = "PrivateID";
}
