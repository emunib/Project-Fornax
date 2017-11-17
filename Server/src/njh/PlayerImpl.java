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
	public boolean JoinGame(ClientPrx client, GamePrx game, Current current) {
		GameImpl gameImpl = (GameImpl) Adapter.findByProxy(game);
		if (gameImpl.AddPlayer(this, client)){
			Game = gameImpl;
			return true;
		}
		return false;
	}

	@Override
	public GameHostPrx CreateGame(ServerPrx server, Current current) {
		GameImpl gameHost = new GameImpl(server, this, Register);
		Game = gameHost;
		Register.AddGame(gameHost);
		return GameHostPrx.checkedCast(Adapter.addWithUUID(gameHost));
	}

	@Override
	public void LeaveGame(Current current) {
		Game.RemovePlayer(this);
		Game = null;
	}

	@Override
	public void LogOut(Current current) {
		_User.Instance = null;
		if (Game != null){
			Game.RemovePlayer(this);
			Game = null;
		}
		Adapter.remove(current.id);
	}
}
