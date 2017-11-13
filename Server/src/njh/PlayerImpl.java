package njh;
import Online.*;
import com.zeroc.Ice.Current;

public class PlayerImpl implements Player {
	@Override
	public PlayerStats GetStats(Current current) {
		return null;
	}

	@Override
	public void JoinGame(ClientPrx client, GamePrx game, Current current) {

	}

	@Override
	public void LeaveGame(Current current) {

	}
}
