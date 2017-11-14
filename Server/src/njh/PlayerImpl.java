package njh;
import Online.*;
import com.zeroc.Ice.Current;
import com.zeroc.Ice.ObjectAdapter;

public class PlayerImpl implements Player {
	User _User;
	ObjectAdapter Adapter;
	GameRegisterImpl Register;
	GameImpl Game;

	public PlayerImpl(User user, ObjectAdapter adapter, GameRegisterImpl register){
		_User = user;
		Adapter = adapter;
		Register = register;
	}
	@Override
	public PlayerStats GetStats(Current current) {
		PlayerStats stats = new PlayerStats();
		stats.Username = _User.Username;
		return stats;
	}

	@Override
	public void JoinGame(ClientPrx client, GamePrx game, Current current) {
		GameImpl gameImpl = (GameImpl) Adapter.findByProxy(game);
		if (gameImpl.AddPlayer(this, client)){
			Game = gameImpl;
		}
	}

	@Override
	public GameHostPrx CreateGame(ServerPrx server, Current current) {
		GameHostImpl gameHost = new GameHostImpl(server, this);
		Game = gameHost;
		Register.AddGame(gameHost);
		return GameHostPrx.checkedCast(Adapter.addWithUUID(gameHost));
	}

	@Override
	public void LeaveGame(Current current) {
		Game.RemovePlayer(this);
	}

	@Override
	public void LogOut(Current current) {

	}
}
