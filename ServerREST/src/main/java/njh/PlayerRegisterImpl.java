package njh;
import Online.*;
import com.zeroc.Ice.*;

public class PlayerRegisterImpl implements PlayerRegister {

	private LobbyListenerPrx Listener;
	private PlayerRegistry PlayerReg;
	private GameRegisterImpl GameRegister;

	public PlayerRegisterImpl(LobbyListenerPrx listenerPrx,
							  GameRegisterImpl gameRegister,
							  PlayerRegistry playerRegistry){
		Listener = listenerPrx;
		PlayerReg = playerRegistry;
		GameRegister = gameRegister;
	}

	@Override
	public PlayerPrx Login(String username, String password, Current current) {
		PlayerPrx playerPrx = PlayerReg.Login(username, password, current);
		GameRegister.ActiveUsers.replace(Listener, playerPrx);
		return  playerPrx;
	}

	@Override
	public PlayerPrx CreateNew(String username, String password, Current current) {
		PlayerPrx playerPrx = PlayerReg.CreateNew(username, password, current);
		GameRegister.ActiveUsers.replace(Listener, playerPrx);
		return  playerPrx;
	}
}
