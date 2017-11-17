package njh;

import Online.PlayerPrx;
import com.zeroc.Ice.Current;
import com.zeroc.Ice.ObjectAdapter;
import com.zeroc.Ice.SystemException;

import java.util.HashMap;

public class PlayerRegistry {
	private HashMap<String, User> UserList;
	private GameRegisterImpl Register;
	private ObjectAdapter Adapter;

	public PlayerRegistry(GameRegisterImpl register, ObjectAdapter adapter){
		Register = register;
		Adapter = adapter;
		UserList = new HashMap<>();
	}

	public PlayerPrx Login(String username, String password, Current current) {
		User user = UserList.get(username);
		if (user == null){
			return null;
		} else if (user.Instance != null) {
			return null;
		}else if (!user.Password.equals(password)){
			return null;
		} else {
			PlayerImpl player = new PlayerImpl(user, Adapter, Register, this);
			PlayerPrx result = PlayerPrx.checkedCast(Adapter.addWithUUID(player));
			user.Instance = player;
			return result;
		}
	}

	public PlayerPrx CreateNew(String username, String password, Current current) {
		User temp = UserList.get(username);
		if (temp == null){
			User newuser = new User(password, username);
			UserList.put(username, newuser);
			PlayerImpl player = new PlayerImpl(newuser, Adapter, Register, this);
			newuser.Instance = player;
			return PlayerPrx.checkedCast(Adapter.addWithUUID(player));
		} else {
			return null;
		}
	}

	public PlayerImpl FindPlayerImplByString(String username){
		User user = UserList.get(username);
		if (user == null) return null;
		return user.Instance;
	}


}
