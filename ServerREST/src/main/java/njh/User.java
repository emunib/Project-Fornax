package njh;

import java.util.HashSet;

public class User {
	public PlayerImpl Instance;
	final String Username;
	final String Password;

	public User(String username, String password){
		Username = username;
		Password = password;
	}
}
