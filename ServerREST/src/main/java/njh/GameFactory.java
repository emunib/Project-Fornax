package njh;

import RESTinterface.Player;

public class GameFactory {
	public final static IdGenerator idGenerator = new IdGenerator();
	public static GameImpl CreateGame(SessionImpl session, RegisterImpl register){
		GameImpl game = new GameImpl(session, register, idGenerator.GetId());
		return game;
	}

	public static void DisposeGame(GameImpl game){
		idGenerator.ReturnId(game.Id);
	}
}
