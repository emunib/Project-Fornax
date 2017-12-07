package njh;
import RESTinterface.Player;
import RESTinterface.PlayerRegister;
import XMLechangeable.Session;
import XMLechangeable.UserLogin;

import javax.ws.rs.core.Response;
import java.util.HashMap;

public class PlayerRegisterImpl implements PlayerRegister {
	private final HashMap<String, PlayerImpl> UserList;

	public PlayerRegisterImpl(){
		UserList = new HashMap<>();
	}

	@Override
	public Session CreateUser(UserLogin userLogin) {
		System.out.println("CreateUser was invoked... ");
		PlayerImpl temp = UserList.get(userLogin.getUsername());
		Session session = new Session();
		if (temp == null){
			PlayerImpl newuser = new PlayerImpl(userLogin.getUsername(), userLogin.getPassword());
			UserList.put(newuser.getUsername(), newuser);
			session = newuser.LoginUser(userLogin);
		}
		System.out.println("...CreateUser is returning");
		return session;
	}

	@Override
	public Player GetUser(String userID) {
		return UserList.get(userID);
	}
}
